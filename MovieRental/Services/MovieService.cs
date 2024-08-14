using Microsoft.EntityFrameworkCore;
using MovieRental.Models;
using MovieRental.DBContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRental.Services
{
    public class MovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            var existingMovie = await _context.Movies.FindAsync(movie.Id);
            if (existingMovie != null)
            {
                existingMovie.Title = movie.Title;
                existingMovie.Description = movie.Description;
                existingMovie.Format = movie.Format;

                _context.Movies.Update(existingMovie);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}
