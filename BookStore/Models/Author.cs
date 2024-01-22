namespace BookStore.Models
{
    public class Author
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FullName { get; set; }
        public virtual List<Book> Books { get; set; }
    }
}