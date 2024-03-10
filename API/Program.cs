using API.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//call our extension method to add in services
//this is done as an extension to simplify program.cs
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

//Middleware (Ability to intercept and modify http requests going out of app and coming into app)
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy"); //This must be before the UseAuthorization call

app.UseAuthorization(); //Middleware to handle authorization (nothing yet)

app.MapControllers(); //Middleware to configure controllers

using var scope = app.Services.CreateScope(); //using means remove scope from memmory when done
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
