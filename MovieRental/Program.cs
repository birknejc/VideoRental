using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Configuration;
using MovieRental.Data;
using MovieRental.DBContext;  // For AppDbContext
using MovieRental.Services;   // For MovieService and OrderService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<OrderService>();

// Connect directly to the SQL Server database
//builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=VideoRental;Trusted_Connection=True;TrustServerCertificate=True;"));


// Connect to PostgreSQL database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
