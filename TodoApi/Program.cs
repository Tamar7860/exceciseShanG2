
using Microsoft.EntityFrameworkCore;
using TodoApi;
using Microsoft.AspNetCore.Cors;
using Swashbuckle.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql("server=localhost;user=root;password=1234;database=firstdb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql")));
// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
});

var app = builder.Build();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Add Swagger middleware
app.UseSwagger();

// Add SwaggerUI middleware
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
});

app.MapGet("/items", async (ToDoDbContext context) =>
{
    return await context.Items.ToListAsync();
});

app.MapPost("/items", async (Item item, ToDoDbContext context) =>
{
    context.Items.Add(item);
    await context.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});
app.MapPut("/items/{id}", async (int id, Item updatedItem, ToDoDbContext context) =>
{
    try{
    var existingItem = await context.Items.FindAsync(id);
    if (existingItem == null)
    {
        return Results.NotFound();
    }

    existingItem.Name = updatedItem.Name; 
    existingItem.AnotherId = updatedItem.AnotherId; 

    await context.SaveChangesAsync();
    
    return Results.NoContent();
    }
       catch (Exception ex)
    {
        // Log the exception or handle it as needed
        Console.WriteLine($"An error occurred: {ex.Message}");

        // Return a response indicating the error
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

app.MapDelete("/items/{id}", async (int id, ToDoDbContext context) =>
{
    var item = await context.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }
    context.Items.Remove(item);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/", () => "AuthServer API is running");
app.Run();
