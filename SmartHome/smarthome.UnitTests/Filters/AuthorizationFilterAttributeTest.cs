using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using smarthome.WebApi.Filters;
using FluentAssertions;

namespace smarthome.UnitTests.Filters
{
    [TestClass]
    public class AuthorizationFilterAttributeTest
    {
        private Mock<HttpContext> _httpContextMock;
        private AuthorizationFilterContext _context;
        private AuthenticationFilterAttribute _attribute;

        public AuthorizationFilterAttributeTest()
        {
            _attribute = new AuthorizationFilterAttribute();
        }

        [TestInitialize]
        public void Initialize()
        {
            _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);

            _context = new AuthorizationFilterContext(
                new ActionContext(
                    _httpContextMock.Object,
                    new RouteData(),
                    new ActionDescriptor()
            ),
                new List<IFilterMetadata>()
            );
        }
        
        [TestMethod]
        public void OnAuthorization_WhenUserHasNotPermission_ShouldReturnUnauthenticatedResponse()
        {
            _httpContextMock.Setup(h => h.Request).Returns(new Mock<HttpRequest>().Object);
            _httpContextMock.Setup(h => h.Request.Headers).Returns(new HeaderDictionary());

            var userLogged = new User()
            {
                Roles = new List<Role>
                {
                    new Role
                    {
                        RoleName = "Administrator",
                        SystemPermissions = new List<SystemPermission>()
                    }
                }
            };

            _httpContextMock.SetupGet(h => h.Items).Returns(new Dictionary<object, object>
            {{ Items.UserLogged, userLogged }});

            var values = new Dictionary<string, object?> { { "action", "test action" }, { "controller", "test controller" } };
            _context.RouteData = new RouteData(new RouteValueDictionary(values));
            _attribute.OnAuthorization(_context);

            var response = _context.Result;

            response.Should().NotBeNull();
            var concreteResponse = response as ObjectResult;
            concreteResponse.Should().NotBeNull();
            concreteResponse.StatusCode.Should().Be((int)401);
            GetInnerCode(concreteResponse.Value).Should().Be("Unauthenticated");
            GetMessage(concreteResponse.Value).Should().Be("You are not authenticated");
        }
        
        [TestMethod]
        public void Should_Build_PermissionKey_Using_Default_When_Not_Provided()
        {
            var values = new Dictionary<string, object?> { { "action", "create" }, { "controller", "companies" } };
            _context.RouteData = new RouteData(new RouteValueDictionary(values));
            _attribute = new AuthorizationFilterAttribute();
            
            var permissionKey = _attribute.GetType()
                .GetMethod("BuildPermission", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_attribute, new object[] { _context }) as PermissionKey;
            
            permissionKey.ToString().Should().Be("create-companies");
        }

        private string GetInnerCode(object value)
        {
            return value.GetType().GetProperty("InnerCode").GetValue(value).ToString();
        }

        private string GetMessage(object value)
        {
            return value.GetType().GetProperty("Message").GetValue(value).ToString();
        }
    }
}