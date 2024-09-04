using LMSApi.App.Atrributes;
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
         

                var userPermissions = claimIdentity.Claims.Where(c => c.Type == "permissions").Select(c => c.Value).ToList();

                if (!userPermissions.Contains(permission))
                {
                    context.Result = new ForbidResult();
                }
            }

        }
    }
}
