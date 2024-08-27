using LMSApi.App.Attributes;
using LMSApi.Database.Data;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Security.Claims;

namespace LMSApi.App.Filters
{
    public class PermissionCheckFilter(ILogger<PermissionCheckFilter> logger, AppDbContext appDbContext) : IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            logger.LogInformation($"Endpoint metadata: {context.ActionDescriptor.EndpointMetadata}");
            var atrribute = context.ActionDescriptor.EndpointMetadata
                .FirstOrDefault(x => x is CheckPermissionAttribute) as CheckPermissionAttribute;

            if (atrribute != null)
            {
                var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimIdentity == null || !claimIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                var permission = atrribute.permissionRouteName;
                logger.LogInformation($"Checking for permission {permission}");

                var userPermissions = claimIdentity.Claims.Where(c => c.Type == "permissions").Select(c => c.Value).ToList();
                foreach (var userPermission in userPermissions)
                {
                    logger.LogInformation($"user has permissions {userPermission}");
                }
                logger.LogInformation($"user has permissions {permission}:{userPermissions.Contains(permission)}");
                if (!userPermissions.Contains(permission))
                {
                    context.Result = new ForbidResult();
                }
            }

        }
    }
}
