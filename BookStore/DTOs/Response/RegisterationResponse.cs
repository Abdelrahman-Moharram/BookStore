namespace BookStore.DTOs.Response
{
    public class RegisterationResponse: AuthResponse
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
    }
}
