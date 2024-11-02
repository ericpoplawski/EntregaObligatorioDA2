using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace smarthome.WebApi.Filters
{
    public class AuthorizationFilterAttribute : AuthenticationFilterAttribute
    {
        private readonly string? _permission;

        public AuthorizationFilterAttribute(string? permission = null)
        {
            _permission = permission;
        }

        public override void OnAuthorization(AuthorizationFilterContext context)
        {
            base.OnAuthorization(context);
            if (context.Result != null)
                return;

            var userLogged = context.HttpContext.Items[Items.UserLogged];

            var userLoggedMapped = (User)userLogged;

            var permissionKey = BuildPermission(context);

            var hasNotPermission = !userLoggedMapped.HasPermission(permissionKey);

            if (hasNotPermission)
            {
                context.Result = new ObjectResult(new
                {
                    InnerCode = "Forbidden",
                    Message = $"Missing permission {permissionKey}"
                })
                {
                    StatusCode = (int)HttpStatusCode.Forbidden
                };
            }
        }
        private PermissionKey BuildPermission(AuthorizationFilterContext context)
        {
            return new PermissionKey(_permission ?? $"{context.RouteData.Values["action"]
                .ToString().ToLower()}-{context.RouteData.Values["controller"].ToString().ToLower()}");
        }
    }
}