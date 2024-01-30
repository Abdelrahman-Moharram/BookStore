using BookStore.Models;

namespace BookStore.DTOs.Book
{
    public class SearchBookDTO
    {
        public string? BookName { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? CategoryName { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
    }
}
