namespace BookStore.DTOs.Response
{
    public class BaseResponse
    {
        public string? Message { get; set; }
        public bool isAuthenticated { get; set; } = false;
    }
}
