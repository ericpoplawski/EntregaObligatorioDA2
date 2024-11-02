using System.Net;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using smarthome.BussinessLogic.Services.Sessions;
using smarthome.WebApi.Filters;

namespace smarthome.Tests.Filters
{
    [TestClass]
    public class AuthenticationFilterAttributeTest
    {
        private Mock<ISessionService> _sessionServiceMock;
        private AuthenticationFilterAttribute _authenticationFilter;
        private AuthorizationFilterContext _authorizationContext;

        [TestInitialize]
        public void SetUp()
        {
            _sessionServiceMock = new Mock<ISessionService>();
            _authenticationFilter = new AuthenticationFilterAttribute();
            
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(ISessionService)))
                .Returns(_sessionServiceMock.Object);

            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProviderMock.Object
            };

            var routeData = new RouteData();

            var actionDescriptor = new ActionDescriptor();

            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            };

            _authorizationContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
        }

        [TestMethod]
        public void OnAuthorization_AllowAnonymous_ShouldNotAuthorize()
        {
            _authorizationContext.ActionDescriptor = new Mock<ActionDescriptor>().Object;
            _authorizationContext.ActionDescriptor.EndpointMetadata = new List<object> { new AllowAnonymousAttribute() };
            
            _authenticationFilter.OnAuthorization(_authorizationContext);
            
            _authorizationContext.Result.Should().BeNull();
        }

        [TestMethod]
        public void OnAuthorization_NoAuthorizationHeader_ShouldReturnUnauthorized()
        {
            _authorizationContext.HttpContext.Request.Headers["Authorization"] = new StringValues(string.Empty);
            
            _authenticationFilter.OnAuthorization(_authorizationContext);
            
            var objectResult = _authorizationContext.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
            objectResult.Value.GetType().GetProperty("InnerCode").GetValue(objectResult.Value).Should().Be("Unauthenticated");
            
        }

        [TestMethod]
        public void OnAuthorization_TokenExpired_ShouldReturnUnauthorized()
        {
            var expiredToken = "expired_token";
            _authorizationContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {expiredToken}";

            _sessionServiceMock.Setup(s => s.IsAuthorizationExpired(expiredToken)).Returns(true);
            
            _authenticationFilter.OnAuthorization(_authorizationContext);
            
            var objectResult = _authorizationContext.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
            objectResult.Value.GetType().GetProperty("InnerCode").GetValue(objectResult.Value).Should().Be("ExpiredAuthorization");
        }

        [TestMethod]
        public void OnAuthorization_InvalidToken_ShouldReturnUnauthorized()
        {
            var invalidToken = "invalid_token";
            _authorizationContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {invalidToken}";

            _sessionServiceMock.Setup(s => s.GetUserByToken(invalidToken)).Returns((User)null);
            
            _authenticationFilter.OnAuthorization(_authorizationContext);
            
            var objectResult = _authorizationContext.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
            objectResult.Value.GetType().GetProperty("InnerCode").GetValue(objectResult.Value).Should().Be("Unauthenticated");
        }

        [TestMethod]
        public void OnAuthorization_ValidToken_ShouldSetUserInContext()
        {
            var validToken = "valid_token";
            var user = new User();
            _authorizationContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {validToken}";

            _sessionServiceMock.Setup(s => s.IsAuthorizationExpired(validToken)).Returns(false);
            _sessionServiceMock.Setup(s => s.GetUserByToken(validToken)).Returns(user);
            
            _authenticationFilter.OnAuthorization(_authorizationContext);
            
            _authorizationContext.Result.Should().BeNull();
            _authorizationContext.HttpContext.Items[Items.UserLogged].Should().Be(user);
        }

        [TestMethod]
        public void OnAuthorization_InternalError_ShouldReturnServerError()
        {
            var validToken = "valid_token";
            _authorizationContext.HttpContext.Request.Headers["Authorization"] = $"Bearer {validToken}";

            _sessionServiceMock.Setup(s => s.IsAuthorizationExpired(validToken)).Returns(false);
            _sessionServiceMock.Setup(s => s.GetUserByToken(validToken)).Throws(new Exception("Unexpected error"));
            
            _authenticationFilter.OnAuthorization(_authorizationContext);
            
            var objectResult = _authorizationContext.Result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            objectResult.Value.GetType().GetProperty("InnerCode").GetValue(objectResult.Value).Should().Be("InternalError");
        }
    }
}
