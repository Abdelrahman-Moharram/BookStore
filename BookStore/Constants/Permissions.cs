namespace BookStore.Constants
{
    public static class Permissions
    {
        
        public static List<string> GeneratePermissionsList(string module, string[] cruds)
        {
            List<string> permissions = new ();
            
            foreach (var op in cruds)
            {
                permissions.Add ($"permissions.{op}.{module}");
            }
            return permissions;
        }
    }
}
