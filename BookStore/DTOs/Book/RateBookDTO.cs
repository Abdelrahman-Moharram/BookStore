namespace BookStore.DTOs.Book
{
    public class RateBookDTO
    {
        public string bookId {  get; set; }

        public int rate { get; set; }
        public string? Message { get; set; }
    }
}
