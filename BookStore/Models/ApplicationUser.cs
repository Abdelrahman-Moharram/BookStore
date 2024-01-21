using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public static implicit operator ApplicationUser(IdentityResult v)
        {
            throw new NotImplementedException();
        }
    }
}
