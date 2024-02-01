namespace BookStore.DTOs.Book
{
    public class BookDetailsDTO:BookDTO
    {
        public DateTime? ReadAt { get; set; }
        public int ReadersCounter { get; set; }
        public decimal? Rate { get; private set; }
    }
}
