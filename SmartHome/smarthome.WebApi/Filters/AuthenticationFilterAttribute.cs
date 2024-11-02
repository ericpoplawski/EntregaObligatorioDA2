using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using smarthome.BussinessLogic.Services.Sessions;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace smarthome.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationFilterAttribute : Attribute, IAuthorizationFilter
    {
        private const string AUTHORIZATION_HEADER = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";

        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .OfType<AllowAnonymousAttribute>().Any();
            
            if (allowAnonymous)
            {
                return;
            }
            
            var authorizationHeader = context.HttpContext.Request.Headers[AUTHORIZATION_HEADER];
            if (StringValues.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.ToString().StartsWith(BEARER_PREFIX))
            {
                context.Result = new ObjectResult
                (new
                {
                    InnerCode = "Unauthenticated",
                    Message = "You are not authenticated"
                })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }
            
            var token = authorizationHeader.ToString().Substring(BEARER_PREFIX.Length);

            var isAuthorizationExpired = IsAuthorizationExpired(token, context);
            if (isAuthorizationExpired)
            {
                context.Result = new ObjectResult(
                    new
                    {
                        InnerCode = "ExpiredAuthorization",
                        Message = "The provided authorization header is expired"
                    })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }

            try
            {
                var userOfAuthorization = GetUserOfAuthorization(token, context);
                
                if (userOfAuthorization == null)
                {
                    context.Result = new ObjectResult(new
                    {
                        InnerCode = "Unauthenticated",
                        Message = "Invalid or expired token"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }

                context.HttpContext.Items[Items.UserLogged] = userOfAuthorization;
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(new
                {
                    InnerCode = "InternalError",
                    Message = $"An error ocurred while processing the request: {ex.Message}"
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
        private bool IsAuthorizationExpired(string token, AuthorizationFilterContext context)
        {
            var sessionService =
                context.HttpContext.RequestServices.GetRequiredService<ISessionService>();
            return sessionService.IsAuthorizationExpired(token);
        }

        private User GetUserOfAuthorization(string token, AuthorizationFilterContext context)
        {
            var sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionService>();
            
            var user = sessionService.GetUserByToken(token);
            
            if (user == null)
            {
                Console.WriteLine("User not found for token: " + token);
            }
            
            return user;
        }
    }
}