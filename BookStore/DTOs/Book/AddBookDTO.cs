using BookStore.Models;

namespace BookStore.DTOs.Book
{
    public class AddBookDTO
    {
        public string Name {get; set;}
        public string? File {get; set;}
        public DateTime PublishDate {get; set;}
        public string CategoryId {get; set;}
        public string AuthorId {get; set;}

    }
}
