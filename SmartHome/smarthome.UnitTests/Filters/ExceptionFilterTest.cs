using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using smarthome.WebApi.Filters;
using Microsoft.AspNetCore.Routing;
using FluentAssertions;
using System.Net;
using Domain.Exceptions;

namespace smarthome.UnitTests.Filters
{
    [TestClass]
    public class ExceptionFilterTest
    {
        private ExceptionContext _context;
        private ExceptionFilter _attribute;

        public ExceptionFilterTest()
        {
            _attribute = new ExceptionFilter();
        }

        [TestInitialize]
        public void Initialize()
        {
            _context = new ExceptionContext(
                new ActionContext(
                    new Mock<HttpContext>().Object,
                    new RouteData(),
                    new ActionDescriptor()),
                new List<IFilterMetadata>());
        }

        [TestMethod]
        public void OnException_WhenExceptionIsNotRegistered_ShouldRespondInternalError()
        {
            _context.Exception = new Exception("Not registered");

            _attribute.OnException(_context);

            var response = _context.Result;

            response.Should().NotBeNull();
            var concreteResponse = response as ObjectResult;
            concreteResponse.Should().NotBeNull();
            concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            GetInnerCode(concreteResponse.Value).Should().Be("InternalError");
            GetMessage(concreteResponse.Value).Should().Be("There was an error when processing the request");
        }

        [TestMethod]
        public void OnException_WhenExceptionIsArgumentNullException_ShouldRespondArgumentNull()
        {
            _context.Exception = new ArgumentNullException("name cannot be null or empty");

            _attribute.OnException(_context);

            var response = _context.Result;

            response.Should().NotBeNull();
            var concreteResponse = response as ObjectResult;
            concreteResponse.Should().NotBeNull();
            concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            GetInnerCode(concreteResponse.Value).Should().Be("ArgumentNull");
            GetMessage(concreteResponse.Value).Should().Be($"Argument can not be null or empty. {_context.Exception.Message}");
        }

        [TestMethod]
        public void OnException_WhenExceptionIsArgumentException_ShouldRespondArgumentNull()
        {
            _context.Exception = new ArgumentException("name cannot be null or empty");

            _attribute.OnException(_context);

            var response = _context.Result;

            response.Should().NotBeNull();
            var concreteResponse = response as ObjectResult;
            concreteResponse.Should().NotBeNull();
            concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            GetInnerCode(concreteResponse.Value).Should().Be("ArgumentError");
            GetMessage(concreteResponse.Value).Should().Be($"Argument is not valid. {_context.Exception.Message}");
        }

        [TestMethod]
        public void OnException_WhenExceptionIsForbiddenException_ShouldRespondForbidden()
        {
            _context.Exception = new ForbiddenException("User does not have permission");

            _attribute.OnException(_context);

            var response = _context.Result;

            response.Should().NotBeNull();
            var concreteResponse = response as ObjectResult;
            concreteResponse.Should().NotBeNull();
            concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
            GetInnerCode(concreteResponse.Value).Should().Be("Forbidden");
            GetMessage(concreteResponse.Value).Should().Be($"Missing permission. {_context.Exception.Message}");
        }

        [TestMethod]
        public void OnException_WhenExceptionIsControllerException_ShouldRespondBadRequest()
        {
            _context.Exception = new ControllerException("Request is null");

            _attribute.OnException(_context);

            var response = _context.Result;

            response.Should().NotBeNull();
            var concreteResponse = response as ObjectResult;
            concreteResponse.Should().NotBeNull();
            concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            GetInnerCode(concreteResponse.Value).Should().Be("ControllerException");
            GetMessage(concreteResponse.Value).Should().Be($"Exception thrown in Controller. {_context.Exception.Message}");
        }

        [TestMethod]
        public void OnException_WhenExceptionIsServiceException_ShouldRespondBadRequest()
        {
            _context.Exception = new ServiceException("Arguments must be 'connected' or 'disconnected'");

            _attribute.OnException(_context);

            var response = _context.Result;

            response.Should().NotBeNull();
            var concreteResponse = response as ObjectResult;
            concreteResponse.Should().NotBeNull();
            concreteResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            GetInnerCode(concreteResponse.Value).Should().Be("ServiceException");
            GetMessage(concreteResponse.Value).Should().Be($"Exception thrown in Service. {_context.Exception.Message}");
        }
        
        
        [TestMethod]
        public void OnException_EntityNotFoundException_ReturnsNotFoundResult()
        {
            var httpContextMock = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContextMock.Object, new RouteData(), new ActionDescriptor());
            var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = new EntityNotFoundException("Test entity not found")
            };
            
            _attribute.OnException(context);
            
            var result = context.Result as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            var value = result.Value;
            var innerCodeProperty = value.GetType().GetProperty("InnerCode");
            var messageProperty = value.GetType().GetProperty("Message");

            innerCodeProperty.Should().NotBeNull();
            messageProperty.Should().NotBeNull();

            var innerCode = innerCodeProperty.GetValue(value).ToString();
            var message = messageProperty.GetValue(value).ToString();

            innerCode.Should().Be("NotFound");
            message.Should().Contain("Test entity not found");
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