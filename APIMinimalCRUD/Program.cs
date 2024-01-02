using APIMinimalCRUD.Context;
using APIMinimalCRUD.Models;
using APIMinimalCRUD.Services;
using APIMinimalCRUD.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BookConnection") ?? "Data Source=Book.db";

builder.Services.AddDbContext<ApiContext>(opt =>
{
    opt.UseSqlite(connectionString);
});
//builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("api"));

builder.Services.AddScoped<IBookService, BookServices>(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ejemplo minimal API", Description = "Codigo de ejemplo" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});

app.MapGet("/api/books", async (ApiContext context) => Results.Ok(await context.BookEntity.ToListAsync()));

app.MapGet("/api/book/{id}", async (int id, ApiContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);

    if (book != null)
    {
        //Regresa una resppuesta 200
        Results.Ok(book);
    } else
    {
        //Regresa un Not Found
        Results.NotFound();
    }
});

app.MapPost("/api/book", async (BookRequets request, IBookService bookService) =>
{
    var bookCreate = await bookService.CreateBook(request);

    return Results.Created($"/books/{bookCreate.Id}", bookCreate);
});

app.MapDelete("/api/book/{id}", async (int id, ApiContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }

    context.BookEntity.Remove(book);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPut("/api/book", async (int id, BookRequets request, ApiContext context) => {
    
    var book = await context.BookEntity.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }

    if(request.Name != null)
    {
        book.Name = request.Name;
    }

    if(request.Isbn != null)
    {
        book.ISBN = request.Isbn;
    }

    await context.SaveChangesAsync();

    //Regresar un 200
    return Results.Ok(book);

});

app.Run();
