namespace BookStore.Models
{
    public class BookReader
    {
        public string userId { get; set; }
        public virtual ApplicationUser? user { get; set; }
        public string BookId { get; set; }
        public virtual Book? Book { get; set; }
        public DateTime ReadAt { get; set; }

        public decimal Rate { get; set; }
    }
}
