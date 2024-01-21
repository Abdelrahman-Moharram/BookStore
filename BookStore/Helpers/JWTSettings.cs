namespace BookStore.Helpers
{
    public class JWTSettings
    {
        public string SECRETKEY { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int DurationInDays { get; set; }
    }
}
