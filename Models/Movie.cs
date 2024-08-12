namespace MovieRental.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Format { get; set; } // DVD or VHS

        // Navigation property for orders that contain this movie
        public List<Order> Orders { get; set; }
    }

}
