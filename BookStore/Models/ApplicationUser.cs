using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<Book>? PublishedBooks { get; set; }
        public virtual List<Book>? ReadBooks { get; set; }
        public virtual List<BookReader>? BookReaders { get; set; }

    }
}
