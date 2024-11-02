using Domain;
using Domain.Exceptions;
using Domain.UserModels;
using FluentAssertions;
using Moq;
using smarthome.Services.System.BusinessLogic;

[TestClass]
public class SystemControllerTest
{
    private SystemController _controller;
    private Mock<ISystemService> _systemServiceMock;

    [TestInitialize]
    public void Initialize()
    {
        _systemServiceMock = new Mock<ISystemService>(MockBehavior.Strict);
        _controller = new SystemController(_systemServiceMock.Object);
    }

    [TestMethod]
    public void CreateAdministrator_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.CreateAdministrator(null);

        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.CreateCompanyOwner(null);
        
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.CreateHomeOwner(null);
        
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    

    [TestMethod]
    public void CreateAdministrator_WhenFirstNameIsNull_ShouldThrowAnException()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = null,
        };

        var act = () => _controller.CreateAdministrator(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'firstName')");
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenFirstNameIsNull_ShouldThrowAnException()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = null,
        };

        var act = () => _controller.CreateCompanyOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'firstName')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenFirstNameIsNull_ShouldThrowAnException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = null,
        };

        var act = () => _controller.CreateHomeOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'firstName')");
    }

    [TestMethod]
    public void CreateAdministrator_WhenFirstNameIsEmpty_ShouldThrowAnException()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = "",
        };

        var act = () => _controller.CreateAdministrator(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'firstName')");
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenFirstNameIsEmpty_ShouldThrowAnException()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = "",
        };

        var act = () => _controller.CreateCompanyOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'firstName')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenFirstNameIsEmpty_ShouldThrowAnException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "",
        };

        var act = () => _controller.CreateHomeOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'firstName')");
    }

    [TestMethod]
    public void CreateAdministrator_WhenlastNameIsNull_ShouldThrowAnException()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = "Leonardo",
            LastName = null,
        };

        var act = () => _controller.CreateAdministrator(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'lastName')");
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenlastNameIsNull_ShouldThrowAnException()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = null,
        };

        var act = () => _controller.CreateCompanyOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'lastName')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenLastNameIsNull_ShouldThrowAnException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = null,
        };

        var act = () => _controller.CreateHomeOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'lastName')");
    }

    [TestMethod]
    public void CreateAdministrator_WhenLastNameIsEmpty_ShouldThrowAnException()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = "Leonardo",
            LastName = "",
        };

        var act = () => _controller.CreateAdministrator(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'lastName')");
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenLastNameIsEmpty_ShouldThrowAnException()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "",
        };

        var act = () => _controller.CreateCompanyOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'lastName')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenLastNameIsEmpty_ShouldThrowAnException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "",
        };

        var act = () => _controller.CreateHomeOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'lastName')");
    }

    [TestMethod]
    public void CreateAdministrator_WhenEmailIsNull_ShouldThrowException()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = null,
        };

        var act = () => _controller.CreateAdministrator(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'email')");
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenEmailIsNull_ShouldThrowException()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = null,
        };

        var act = () => _controller.CreateCompanyOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'email')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenEmailIsNull_ShouldThrowException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = null,
        };

        var act = () => _controller.CreateHomeOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'email')");
    }

    [TestMethod]
    public void CreateAdministrator_WhenEmailIsEmpty_ShouldThrowException()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "",
        };

        var act = () => _controller.CreateAdministrator(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'email')");
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenEmailIsEmpty_ShouldThrowException()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "",
        };

        var act = () => _controller.CreateCompanyOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'email')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenEmailIsEmpty_ShouldThrowException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "",
        };

        var act = () => _controller.CreateHomeOwner(request);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'email')");
    }

    [TestMethod]
    public void CreateAdministrator_WhenPasswordIsNull_ShouldThrowException()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "leofernandez10@gmail.com",
            Password = null,
        };

        var act = () => _controller.CreateAdministrator(request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'password')");

    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenPasswordIsNull_ShouldThrowException()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "mail@mail.com",
            Password = null,
        };
        
        var act = () => _controller.CreateCompanyOwner(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'password')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenPasswordIsNull_ShouldThrowException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "mail@mail.com",
            Password = null
        };
        
        var act = () => _controller.CreateHomeOwner(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'password')");
    }        

    [TestMethod]
    public void CreateAdministrator_WhenPasswordIsEmpty_ShouldThrowException()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "leofernandez10@gmail.com",
            Password = "",
        };

        var act = () => _controller.CreateAdministrator(request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'password')");

    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenPasswordIsEmpty_ShouldThrowException()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "mail@mail.com",
            Password = ""
        };
        
        var act = () => _controller.CreateCompanyOwner(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'password')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenPasswordIsEmpty_ShouldThrowException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "mail@mail.com",
            Password = ""
        };
        
        var act = () => _controller.CreateHomeOwner(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'password')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenProfilePictureIsNull_ShouldNotThrowException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "mail@mail.com",
            Password = "Pepito1234",
            ProfilePicture = null
        };
        
        var act = () => _controller.CreateHomeOwner(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'profilePicture')");
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenProfilePictureIsEmpty_ShouldNotThrowException()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "mail@mail.com",
            Password = "Pepito1234",
            ProfilePicture = ""
        };
        
        var act = () => _controller.CreateHomeOwner(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'profilePicture')");
    }

    [TestMethod]
    public void CreateAdministrator_WhenRequestIsValid_ShouldCreateAdministrator()
    {
        var request = new CreateAdministratorRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "leofernandez10@gmail.com",
            Password = "$Leitodelcap1891"
        };

        var expectedAdmin = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        var act = () => _controller.CreateAdministrator(request);
        _systemServiceMock.Setup(m => m.AddAdministrator(It.IsAny<CreateUserArguments>())).Returns(expectedAdmin);
        var response = _controller.CreateAdministrator(request);

        response.Should().NotBeNull();
        response.Id.Should().Be(expectedAdmin.Id);
        _systemServiceMock.VerifyAll();

    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenRequestIsValid_ShouldCreateCompanyOwner()
    {
        var request = new CreateCompanyOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "mail@mail.com",
            Password = "$Leitodelcap1891"
        };
        
        var expectedCompanyOwner = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };
        
        var act = () => _controller.CreateCompanyOwner(request);
        _systemServiceMock.Setup(m => m.AddCompanyOwner(It.IsAny<CreateUserArguments>())).Returns(expectedCompanyOwner);
        var response = _controller.CreateCompanyOwner(request);
        
        response.Should().NotBeNull();
        response.Id.Should().Be(expectedCompanyOwner.Id);
        _systemServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenRequestIsValid_ShouldCreateHomeOwner()
    {
        var request = new CreateHomeOwnerRequest()
        {
            FirstName = "Leonardo",
            LastName = "Fernandez",
            Email = "mail@mail.com",
            Password = "$Leitodelcap1891",
            ProfilePicture = "https://www.google.com"
        };
        
        var expectedHomeOwner = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            ProfilePicture = request.ProfilePicture
        };
        
        var act = () => _controller.CreateHomeOwner(request);
        _systemServiceMock.Setup(m => m.AddHomeOwner(It.IsAny<CreateHomeOwnerArguments>())).Returns(expectedHomeOwner);
        var response = _controller.CreateHomeOwner(request);
        
        response.Should().NotBeNull();
        response.Id.Should().Be(expectedHomeOwner.Id);
        _systemServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetUsers_ShouldReturnUsers()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var roleName = "admin";
        var fullName = "John Doe";

        var users = new List<User>
        {
            new User
            {
                FirstName = "John",
                LastName = "Doe",
                FullName = "John Doe",
                Roles = new List<Role>()
                {
                    new Role { RoleName = "admin" }
                },
                CreationDate = DateTime.Now
            }
        };

        _systemServiceMock
            .Setup(x => x.GetUsers(It.Is<GetAllUsersArguments>(
                a => a.FullName == fullName &&
                     a.RoleName == roleName &&
                     a.PageNumber == pageNumber &&
                     a.PageSize == pageSize)))
            .Returns(users)
            .Verifiable();

        var result = _controller.GetUsers(pageNumber, pageSize, roleName, fullName);

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result[0].FullName.Should().Be("John Doe");
        result[0].RoleNames[0].Should().Be("admin");

        _systemServiceMock.VerifyAll();
    }

    [TestMethod]
    public void DeleteAdministratorById_WhenIdIsNotValid_ShouldThrowException()
    {
        var id = "invalidId";

        var act = () => _controller.DeleteAdministratorById(id);

        act.Should().Throw<ControllerException>().WithMessage("Id is not valid");
    }

    [TestMethod]
    public void DeleteAdministratorById_WhenIdIsValid_ShouldDeleteBuilding()
    {
        var id = Guid.NewGuid().ToString();

        _systemServiceMock.Setup(m => m.DeleteAdministratorById(It.IsAny<string>()));

        var act = () => _controller.DeleteAdministratorById(id);

        act.Should().NotThrow<Exception>();

        _systemServiceMock.VerifyAll();
    }

    [TestMethod]
    public void AddHomeOwnerRoleToUser_ShouldAddHomeOwnerRoleToUser()
    {
        var id = Guid.NewGuid().ToString();

        _systemServiceMock.Setup(m => m.AddHomeOwnerRoleToUser(It.IsAny<string>()));

        var act = () => _controller.AddHomeOwnerRoleToUser(id);

        act.Should().NotThrow<Exception>();

        _systemServiceMock.VerifyAll();
    }
}