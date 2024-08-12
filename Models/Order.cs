namespace MovieRental.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderID { get; set; } 
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public List<Movie> Movies { get; set; } // Navigation property for multiple movies
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
