namespace BookStore.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public virtual List<Book>? Books { get; set; }
    }
}