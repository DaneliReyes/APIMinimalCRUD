

namespace APIMinimalCRUD.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ISBN { get; set; }
    }
}

public record BookRequets(string Name, string Isbn);