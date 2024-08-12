using MovieRental.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieRental.Services
{
    public class CartService
    {
        private readonly List<Movie> _cart = new List<Movie>();

        public IEnumerable<Movie> GetCartItems()
        {
            return _cart;
        }

        public void AddToCart(Movie movie)
        {
            if (!_cart.Any(m => m.Id == movie.Id))
            {
                _cart.Add(movie);
            }
        }

        public void AddToCart(IEnumerable<Movie> movies)
        {
            foreach (var movie in movies)
            {
                AddToCart(movie);
            }
        }

        public void RemoveFromCart(int movieId)
        {
            var movie = _cart.FirstOrDefault(m => m.Id == movieId);
            if (movie != null)
            {
                _cart.Remove(movie);
            }
        }

        public void ClearCart()
        {
            _cart.Clear();
        }
    }
}
