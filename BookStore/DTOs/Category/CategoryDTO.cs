using BookStore.DTOs.Book;

namespace BookStore.DTOs.Category
{
    public class CategoryDTO
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }
        public virtual List<BookDTO>? Books { get; set; }
    }
}
