namespace BookStore.Models
{
    public class UploadedFile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? FileName { get; set; }
        public string? ContentType { get; set; }

        public string bookId { get; set; }
        public Book Book { get; set; }
    }
}
