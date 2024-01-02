using APIMinimalCRUD.Models;

namespace APIMinimalCRUD.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> CreateBook(BookRequets request);
    }
}
