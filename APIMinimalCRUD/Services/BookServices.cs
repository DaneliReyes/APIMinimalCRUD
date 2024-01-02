using APIMinimalCRUD.Context;
using APIMinimalCRUD.Models;
using APIMinimalCRUD.Services.Interfaces;

namespace APIMinimalCRUD.Services
{
    public class BookServices : IBookService
    {
        private readonly ApiContext _context;

        //Crear constructor ctor
        public BookServices(ApiContext context)
        {
            _context = context;
        }

        public async Task<Book> CreateBook(BookRequets request)
        {
            var book = new Book
            {
                //Si requets.Name es null, se devuelve un string vacio
                Name = request.Name ?? string.Empty,
                ISBN = request.Isbn ?? string.Empty,
            };

            var bookCreate = await _context.BookEntity.AddAsync(book);

            await _context.SaveChangesAsync();

            return bookCreate.Entity;
        }
    }
}
