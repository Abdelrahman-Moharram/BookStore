using Microsoft.AspNetCore.Authorization;

namespace BookStore.Filters
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; private set; }
    }
}
