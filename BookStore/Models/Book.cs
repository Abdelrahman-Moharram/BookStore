namespace BookStore.Models
{
    public class Book
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
        public string? File { get; set; }
        public DateTime PublishDate { get; set; }


        public string CategoryId { get; set; }
        public virtual Category? Category { get; set; }


        public string AuthorId { get; set; }
        public virtual Author? Author { get; set; }
        
        public string PublisherId { get; set; }
        public virtual ApplicationUser? Publisher { get; set; }

        public virtual List<ApplicationUser>? Readers { get; set; }
        public virtual List<BookReader>? BookReaders { get; set; }

        public int RatersCount { get; set; }
        public decimal RatersTotal { get; set;}
    }
}
