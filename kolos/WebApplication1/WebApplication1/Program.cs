using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();