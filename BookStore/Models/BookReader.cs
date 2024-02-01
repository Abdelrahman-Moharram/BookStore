namespace BookStore.Models
{
    public class BookReader
    {
        public string userId { get; set; }
        public virtual ApplicationUser? user { get; set; }
        public string BookId { get; set; }
        public virtual Book? Book { get; set; }
        public DateTime ReadAt { get; set; }
        public decimal Rate { get; private set; }

        public void RateBook(decimal val)
        {
            Rate = val;
            
        }

        public void UpdateBookRate(decimal[] rates)
        {
            var ratersCount = rates.Count();
            Book.RatersCount = ratersCount;
            Book.RatersTotal = rates.Sum() / ratersCount;
        }
    }
}
