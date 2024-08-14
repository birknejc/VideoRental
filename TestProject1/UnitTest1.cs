using Bunit;
using Microsoft.EntityFrameworkCore;
using MovieRental.Models;
using MovieRental.Pages;
using MovieRental.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Bunit.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using MovieRental.DBContext;

public class CatalogAndCartPageTests : TestContext
{
    [Fact]
    public async Task Catalog_Should_Add_Selected_Movies_To_Cart()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Movie 1", Description = "Description 1", Format = "DVD" },
            new Movie { Id = 2, Title = "Movie 2", Description = "Description 2", Format = "VHS" }
        };

        var movieService = new MovieService(CreateInMemoryDbContext(movies));
        var cartService = new CartService();

        Services.AddSingleton(movieService);
        Services.AddSingleton(cartService);

        // Render the Catalog component
        var catalogComponent = RenderComponent<Catalog>();

        // Act
        // Select the first movie
        catalogComponent.Find("input[type=checkbox]").Change(true);

        // Click the "Add Selected to Cart" button
        var addToCartButton = catalogComponent.Find("button.btn-primary");
        await addToCartButton.ClickAsync(new MouseEventArgs()); // Pass MouseEventArgs

        // Assert
        var cartItems = cartService.GetCartItems().ToList();
        Assert.Single(cartItems);
        Assert.Equal("Movie 1", cartItems.First().Title);
    }

    [Fact]
    public void CartPage_Should_Display_Added_Movies()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Movie 1", Description = "Description 1", Format = "DVD" }
        };

        var cartService = new CartService();
        cartService.AddToCart(movies.First());

        // Set up an in-memory DbContext for OrderService
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new AppDbContext(options);
        var orderService = new OrderService(context);

        Services.AddSingleton(cartService);
        Services.AddSingleton(orderService);

        // Render the CartPage component
        var cartComponent = RenderComponent<CartPage>();

        // Act
        var movieTitle = cartComponent.Find("h5.card-title").TextContent;

        // Assert
        Assert.Equal("Movie 1", movieTitle);
    }

    [Fact]
    public async Task CartPage_Should_Place_Order_Successfully()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Movie 1", Description = "Description 1", Format = "DVD" }
        };

        var cartService = new CartService();
        cartService.AddToCart(movies.First());

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
            .Options;

        var context = new AppDbContext(options);
        var orderService = new OrderService(context);

        Services.AddSingleton(cartService);
        Services.AddSingleton(orderService);

        // Render the CartPage component
        var cartComponent = RenderComponent<CartPage>();

        // Act
        cartComponent.Find("input[id='customerName']").Change("John Doe");
        cartComponent.Find("input[id='customerEmail']").Change("john.doe@example.com");
        await cartComponent.Find("button[type='submit']").ClickAsync(new MouseEventArgs()); // Pass MouseEventArgs

        // Wait for the async operation to complete and the UI to update
        await Task.Delay(1000); // Adjust delay as needed

        // Assert
        var successMessage = cartComponent.Find("div.alert-success");
        Assert.NotNull(successMessage);
        Assert.Contains("Order completed successfully!", successMessage.TextContent);

        var cartItems = cartService.GetCartItems().ToList();
        Assert.Empty(cartItems);
    }

    // Helper method to create an in-memory DbContext
    private AppDbContext CreateInMemoryDbContext(IEnumerable<Movie> movies)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new AppDbContext(options);
        context.Movies.AddRange(movies);
        context.SaveChanges();

        return context;
    }
}
