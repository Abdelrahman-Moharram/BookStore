using BookStore.Models;

namespace BookStore.DTOs.Book
{
    public class BookDTO
    {

        public string? BookName { get; set; }
        public string? File { get; set; }
        public DateTime PublishDate { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherId { get; set; }
        public string? PublisherName { get; set; }

    }
}
