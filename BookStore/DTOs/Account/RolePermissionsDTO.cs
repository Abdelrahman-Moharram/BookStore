namespace BookStore.DTOs.Account
{
    public class RolePermissionsDTO
    {
        public string RoleId { get; set; }
        public List<string> Permissions { get; set;}
    }
}
