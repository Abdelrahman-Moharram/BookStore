﻿using Microsoft.AspNetCore.Authorization;

namespace BookStore.Filters
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
         public string Permission { get; private set; }
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }

       
    }
}
