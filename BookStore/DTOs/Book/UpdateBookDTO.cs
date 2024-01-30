namespace BookStore.DTOs.Book
{
    public class UpdateBookDTO:AddBookDTO
    {
        public string Id { get; set; }
        public string PublisherId { get; set; }

    }
}
