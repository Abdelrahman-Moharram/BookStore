using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.Account
{
    public class RegisterDTO : LoginDTO
    {

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
