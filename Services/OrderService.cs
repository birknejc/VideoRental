using Microsoft.EntityFrameworkCore;
using MovieRental.Models;
using MovieRental.DBContext;
using System;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace MovieRental.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task PlaceOrderAsync(Order order)
        {
            try
            {
                order.OrderDate = order.OrderDate.ToUniversalTime();
                order.TotalAmount = order.Movies.Sum(m => GetMoviePrice(m.Format));
                order.OrderID = Guid.NewGuid().ToString();

                var trackedMovies = new List<Movie>();

                foreach (var movie in order.Movies)
                {
                    var existingMovie = await _context.Movies.FindAsync(movie.Id);
                    if (existingMovie != null)
                    {
                        trackedMovies.Add(existingMovie);
                    }
                    else
                    {
                        throw new Exception($"Movie with ID {movie.Id} does not exist in the database.");
                    }
                }

                order.Movies = trackedMovies;

                _context.Orders.Add(order);
                Console.WriteLine($"Order added to the context for customer: {order.CustomerName}, Total: {order.TotalAmount}.");
                await _context.SaveChangesAsync();
                Console.WriteLine("Order saved to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PlaceOrderAsync: {ex.Message}");
                throw;
            }
        }



        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(o => o.Movies).ToListAsync();
        }

        private decimal GetMoviePrice(string format)
        {
            return format == "VHS" ? 3m : 5m;
        }
    }
}
