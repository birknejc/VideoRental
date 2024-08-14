using Microsoft.EntityFrameworkCore;
using MovieRental.Models;
using MovieRental.DBContext;
using System;

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
            order.OrderDate = DateTime.Now;
            order.TotalAmount = order.Movies.Sum(m => GetMoviePrice(m.Format));

            // Generate a unique OrderID
            order.OrderID = Guid.NewGuid().ToString();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
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
