using Domain;

using FluentAssertions;
using Moq;
using smarthome.BussinessLogic.Services.Sessions;
using smarthome.Services.System.BusinessLogic;
using System.Linq.Expressions;
using Domain.SessionModels;
using Domain.Exceptions;

namespace smarthome.UnitTests.Sessions
{
    [TestClass]
    public class SessionServiceTest
    {
        private ISessionService _service;
        private Mock<ISystemService> _systemServiceMock;
        private Mock<IRepository<Session>> _repositoryMock;

        [TestInitialize]
        public void Initialize()
        {
            _systemServiceMock = new Mock<ISystemService>();
            _repositoryMock = new Mock<IRepository<Session>>();
            _service = new SessionService(_systemServiceMock.Object, _repositoryMock.Object);
        }

        [TestMethod]
        public void CreateSession_WhenUserDoesNotExist_ShouldThrowAnException()
        {
            CreateSessionArguments arguments = new CreateSessionArguments
                ("manyapadreydecano@gmail.com", "$Leitodelcap1891");
            _systemServiceMock.Setup(u => u.GetUserByEmail(It.IsAny<string>())).Returns((User)null);
            var act = () => _service.CreateSession(arguments);

            act.Should().Throw<EntityNotFoundException>().
                WithMessage("User not found");
            _systemServiceMock.VerifyAll();
        }

        [TestMethod]
        public void CreateSession_WhenPasswordIsIncorrect_ShouldThrowAnException()
        {
            CreateSessionArguments arguments = new CreateSessionArguments
                ("manyapadreydecano@gmail.com", "$Leitodelcap1891");
            var user = new User()
            {
                Password = "$FedeManya1891"
            };
            _systemServiceMock.Setup(u => u.GetUserByEmail(It.IsAny<string>())).Returns(user);

            var act = () => _service.CreateSession(arguments);
            act.Should().Throw<Exception>().WithMessage("Invalid password");
            _systemServiceMock.VerifyAll();
        }

        [TestMethod]
        public void CreateSession_WhenCredentialsAreValid_ShouldReturnToken()
        {
            CreateSessionArguments arguments = new CreateSessionArguments
                ("manyapadreydecano@gmail.com", "$Leitodelcap1891");
            var user = new User()
            {
                Password = "$Leitodelcap1891"
            };
            _systemServiceMock.Setup(u => u.GetUserByEmail(It.IsAny<string>())).Returns(user);

            var result = () => _service.CreateSession(arguments);
            result.Should().NotBeNull();
            result.Should().NotThrow<Exception>();
            _systemServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetUserByToken_WhenHeaderIsInvalid_ShouldThrowAnException()
        {
            var invalidToken = "invalid_token";
            Action act = () => _service.GetUserByToken(invalidToken);

            act.Should().Throw<EntityNotFoundException>()
                .WithMessage("Session not found");

            _repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetUserByToken_WhenHeaderIsValid_ShouldReturnUser()
        {
            var header = "validHeader";
            var session = new Session()
            {
                Token = header,
                User = new User() { Roles = new List<Role>{ new Role() { SystemPermissions = new List<SystemPermission>() } } }
            };

            _repositoryMock.Setup(r => r.Get(
                It.Is<Expression<Func<Session, bool>>>(s => s.Compile().Invoke(new Session { Token = header })),
                It.IsAny<Expression<Func<Session, object>>>(),
                It.IsAny<Expression<Func<Session, object>>>()
            )).Returns(session);

            var result = _service.GetUserByToken(header);

            result.Should().NotBeNull();
            result.Should().Be(session.User);

            _repositoryMock.VerifyAll();
        }

        [TestMethod]
        public void IsAuthorizationExpired_WhenNoSessionsHaveAuthorizationHeaderAsToken_ShouldReturnTrue()
        {
            string authorizationHeader = "token12345";
            _repositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Session, bool>>>())).Returns((Session)null);
            bool result = _service.IsAuthorizationExpired(authorizationHeader);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void IsAuthorizationExpired_WhenSessionHasAuthorizationHeaderAsToken_ShouldReturnFalse()
        {
            string authorizationHeader = "token12345";
            var session = new Session()
            {
                Token = authorizationHeader
            };
            _repositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            bool result = _service.IsAuthorizationExpired(authorizationHeader);

            result.Should().BeFalse();
        }
        
        [TestMethod]
        public void Session_Id_Should_Get_And_Set_Value()
        {
            var session = new Session();
            var id = Guid.NewGuid().ToString();

            session.Id = id;

            session.Id.Should().Be(id);
        }

        [TestMethod]
        public void Session_Token_Should_Get_And_Set_Value()
        {
            var session = new Session();
            var token = "testToken123";

            session.Token = token;

            session.Token.Should().Be(token);
        }

        [TestMethod]
        public void Session_UserId_Should_Get_And_Set_Value()
        {
            var session = new Session();
            var userId = "userId123";

            session.UserId = userId;

            session.UserId.Should().Be(userId);
        }

        [TestMethod]
        public void Session_User_Should_Get_And_Set_Value()
        {
            var session = new Session();
            var user = new User { Id = "userId", FirstName = "John", LastName = "Doe" };

            session.User = user;

            session.User.Should().Be(user);
        }

        [TestMethod]
        public void SessionUserResponse_Id_Should_Return_Initialized_Value()
        {
            var id = "testId";
            var email = "test@example.com";
            var roleNames = new List<string> { "HomeOwner" } ;

            var response = new SessionUserResponse
            {
                Id = id,
                Email = email,
                RoleNames = roleNames
            };

            response.Id.Should().Be(id);
        }
        
        [TestMethod]
        public void SessionUserResponse_Email_Should_Return_Initialized_Value()
        {
            var id = "testId";
            var email = "test@example.com";
            var roleNames = new List<string> { "HomeOwner" };

            var response = new SessionUserResponse
            {
                Id = id,
                Email = email,
                RoleNames = roleNames
            };

            response.Email.Should().Be(email);
        }
        
        [TestMethod]
        public void SessionUserResponse_RoleId_Should_Return_Initialized_Value()
        {
            var id = "testId";
            var email = "test@example.com";
            var roleNames = new List<string> { "HomeOwner" };

            var response = new SessionUserResponse
            {
                Id = id,
                Email = email,
                RoleNames = roleNames
            };

            response.RoleNames[0].Should().Be(roleNames[0]);
        }

        [TestMethod]
        public void SessionUserResponse_Constructor_Should_Initialize_Properties_Correctly()
        {
            var id = "testId";
            var email = "test@example.com";
            var roleNames = new List<string> { "HomeOwner" };

            var response = new SessionUserResponse
            {
                Id = id,
                Email = email,
                RoleNames = roleNames
            };

            response.Id.Should().Be(id);
            response.Email.Should().Be(email);
            response.RoleNames[0].Should().Be(roleNames[0]);
        }
    }
}