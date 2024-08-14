using Bunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieRental.Models;
using MovieRental.Pages;
using MovieRental.Services;
using MovieRental.DBContext; // Ensure this namespace is correct
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Components.Web; // For MouseEventArgs

public class MovieFormTests : TestContext
{
    public MovieFormTests()
    {
        // Register services with the TestContext
        Services.AddSingleton(CreateMovieService());
    }



    [Fact]
    public async Task MovieForm_Should_Delete_Movie_Successfully()
    {
        // Arrange
        var movieToDelete = new Movie { Id = 1, Title = "Movie to Delete", Description = "Description", Format = "DVD" };
        var movieService = CreateMovieService(new List<Movie> { movieToDelete });

        // Register services with the TestContext
        Services.AddSingleton(movieService);

        // Render the MovieForm component
        var movieFormComponent = RenderComponent<MovieForm>();

        // Act
        var deleteButton = movieFormComponent.Find("button.btn-danger");
        await deleteButton.ClickAsync(new MouseEventArgs());

        // Wait for the async operation to complete and the UI to update
        await Task.Delay(1000);

        // Assert
        var movies = await movieService.GetMoviesAsync();
        Assert.Empty(movies);
    }

    private MovieService CreateMovieService(IEnumerable<Movie> movies = null)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid())
            .Options;

        var context = new AppDbContext(options);
        if (movies != null)
        {
            context.Movies.AddRange(movies);
            context.SaveChanges();
        }

        return new MovieService(context);
    }
}
