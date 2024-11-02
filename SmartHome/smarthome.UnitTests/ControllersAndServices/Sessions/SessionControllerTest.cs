using Domain;
using Domain.SessionModels;
using FluentAssertions;
using Moq;
using smarthome.BussinessLogic.Services.Sessions;
using smarthome.WebApi.Controllers;

namespace smarthome.UnitTests
{
    [TestClass]
    public class SessionControllerTest
    {
        private SessionController _controller;
        private Mock<ISessionService> _sessionServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);
            _controller = new SessionController(_sessionServiceMock.Object);
        }

        [TestMethod]
        public void Create_WhenRequestIsNull_ShouldThrowAnException()
        {
            var act = () => _controller.CreateSession(null);

            act.Should().Throw<Exception>().WithMessage("Request cannot be null");
        }

        [TestMethod]
        public void Create_WhenEmailIsNull_ShouldThrowAnException()
        {
            CreateSessionRequest request = new CreateSessionRequest()
            {
                Email = null
            };
            var act = () => _controller.CreateSession(request);

            act.Should().Throw<ArgumentNullException>().
                WithMessage("Value of parameter cannot be null or empty (Parameter 'email')");
        }

        [TestMethod]
        public void Create_WhenEmailIsEmpty_ShouldThrowAnException()
        {
            CreateSessionRequest request = new CreateSessionRequest()
            {
                Email = ""
            };
            var act = () => _controller.CreateSession(request);

            act.Should().Throw<ArgumentNullException>().
                WithMessage("Value of parameter cannot be null or empty (Parameter 'email')");
        }

        [TestMethod]
        public void Create_WhenPasswordIsNull_ShouldThrowAnException()
        {
            CreateSessionRequest request = new CreateSessionRequest()
            {
                Email = "washingtonaguerre@gmail.com",
                Password = null
            };
            var act = () => _controller.CreateSession(request);

            act.Should().Throw<ArgumentNullException>().
                WithMessage("Value of parameter cannot be null or empty (Parameter 'password')");
        }

        [TestMethod]
        public void Create_WhenPasswordIsEmpty_ShouldThrowAnException()
        {
            CreateSessionRequest request = new CreateSessionRequest()
            {
                Email = "washingtonaguerre@gmail.com",
                Password = ""
            };
            var act = () => _controller.CreateSession(request);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'password')");
        }

        [TestMethod]
        public void Create_WhenRequestIsValid_ShouldReturnResponse()
        {
            CreateSessionRequest request = new CreateSessionRequest()
            {
                Email = "washingtonaguerre@gmail.com",
                Password = "$WashingtonAguerre1891"
            };

            string token = "token";

            var user = new User()
            {
                Id = "ABCDE12345",
                Email = "washingtonaguerre@gmail.com",
                Roles = new List<Role>
                {
                    new Role()
                    {
                        Id = "12345ABCDE"
                    }
                },
            };

            _sessionServiceMock.Setup(s => s.CreateSession(It.IsAny<CreateSessionArguments>())).Returns(token);
            _sessionServiceMock.Setup(s => s.GetUserByToken(It.IsAny<string>())).Returns(user);

            var response = _controller.CreateSession(request);
            response.Should().NotBeNull();
            response.Token.Should().Be(token);
            response.User.Id.Should().Be(user.Id);
            response.User.RoleNames[0].Should().Be(user.Roles[0].RoleName);
        }
    }
}