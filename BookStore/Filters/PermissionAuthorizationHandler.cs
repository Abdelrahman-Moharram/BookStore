using BookStore.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BookStore.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly JWTSettings _jwt;

        public PermissionAuthorizationHandler(
            IOptions<JWTSettings> jwt
            )
        {
            _jwt = jwt.Value;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
                return;
            var canAccess = context
                .User
                    .Claims
                        .Any(
                            c =>
                            c.Type == "Permission" &&
                            c.Value == requirement.Permission &&
                            c.Issuer == _jwt.Issuer
                        );

            if (canAccess)
                context.Succeed(requirement);
            return;

        }
    }
}
