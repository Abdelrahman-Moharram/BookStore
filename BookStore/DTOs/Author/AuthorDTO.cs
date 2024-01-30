using BookStore.DTOs.Book;

namespace BookStore.DTOs.Author
{
    public class AuthorDTO
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }

        public List<BookDTO>? Books { get; set; }
    }
}
