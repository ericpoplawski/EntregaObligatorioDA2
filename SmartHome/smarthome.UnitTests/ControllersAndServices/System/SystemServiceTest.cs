using smarthome.BussinessLogic;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using smarthome.BussinessLogic.Services.System;
using Domain.UserModels;
using Domain;
using Domain.Exceptions;
using smarthome.BussinessLogic.Services.HomeServices;

[TestClass]
public sealed class SystemServiceTest
{
    private SystemService _service;
    private Mock<IRepository<User>> _mockUserRepository;
    private Mock<IRepository<Role>> _mockRoleRepository;
    
    [TestInitialize]
    public void Initialize(){
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockRoleRepository = new Mock<IRepository<Role>>();
        _service = new SystemService
            (_mockUserRepository.Object, 
            _mockRoleRepository.Object);
    }

    [TestMethod]
    public void CreateAdministrator_WhenEmailFormatIsInvalid_ShouldThrowException()
    {
        var request = new CreateAdministratorRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mailmail.com",
            Password = "password"
        };

        var arguments = new CreateAdministratorArguments(request.FirstName, request.LastName,
            request.Email, request.Password);

        var act = () => _service.AddAdministrator(arguments);

        act.Should().Throw<ArgumentException>().WithMessage("Incorrect format (Parameter 'email')");
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenEmailFormatIsInvalid_ShouldThrowException()
    {
        var request = new CreateCompanyOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mailmail.com",
            Password = "password",
        };

        var arguments = new CreateCompanyOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password);

        var act = () => _service.AddCompanyOwner(arguments);

        act.Should().Throw<ArgumentException>().WithMessage("Incorrect format (Parameter 'email')");
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenEmailFormatIsInvalid_ShouldThrowException()
    {
        var request = new CreateHomeOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mailmail.com",
            Password = "password",
            ProfilePicture = "www.fotojpg.com"
        };

        var arguments = new CreateHomeOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password, request.ProfilePicture);

        var act = () => _service.AddHomeOwner(arguments);

        act.Should().Throw<ArgumentException>().WithMessage("Incorrect format (Parameter 'email')");
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void CreateAdministrator_WhenPasswordFormatIsInvalid_ShouldThrowException()
    {
        var request = new CreateAdministratorRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "password"
        };

        var arguments = new CreateAdministratorArguments(request.FirstName, request.LastName,
            request.Email, request.Password);

        var act = () => _service.AddAdministrator(arguments);

        act.Should().Throw<ArgumentException>().
            WithMessage("Must have at least one number and special character (Parameter 'password')");
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void CreateCompanyOwner_WhenPasswordFormatIsInvalid_ShouldThrowException()
    {
        var request = new CreateCompanyOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "password",
        };
        
        var arguments = new CreateCompanyOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password);
        
        var act = () => _service.AddCompanyOwner(arguments);
        
        act.Should().Throw<ArgumentException>().
            WithMessage("Must have at least one number and special character (Parameter 'password')");
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenPasswordFormatIsInvalid_ShouldThrowException()
    {
        var request = new CreateHomeOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "password",
            ProfilePicture = "www.jpg.com"
        };
        
        var arguments = new CreateHomeOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password, request.ProfilePicture);
        
        var act = () => _service.AddHomeOwner(arguments);
        
        act.Should().Throw<ArgumentException>().
            WithMessage("Must have at least one number and special character (Parameter 'password')");
        
        _mockUserRepository.VerifyAll();
    }        

    [TestMethod]
    public void CreateAdministrator_WhenEmailAlreadyExists_ShouldThrowException(){
        
        var request = new CreateAdministratorRequest{
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "@password1"
        };

        var arguments = new CreateAdministratorArguments(request.FirstName, request.LastName, 
            request.Email, request.Password);

        _mockUserRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);

        var act = () => _service.AddAdministrator(arguments);

        act.Should().Throw<ServiceException>().WithMessage("There is already a user with this email");
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenEmailAlreadyExists_ShouldThrowException()
    {

        var request = new CreateCompanyOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "@password1",
        };
        
        var arguments = new CreateCompanyOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password);
        
        _mockUserRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);
        
        var act = () => _service.AddCompanyOwner(arguments);
        
        act.Should().Throw<ServiceException>().WithMessage("There is already a user with this email");
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenEmailAlreadyExists_ShouldThrowException()
    {
        var request = new CreateHomeOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "@password1",
            ProfilePicture = "www.jpg.com"
        };
        
        var arguments = new CreateHomeOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password, request.ProfilePicture);
        
        _mockUserRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);
        
        var act = () => _service.AddHomeOwner(arguments);
        
        act.Should().Throw<ServiceException>().WithMessage("There is already a user with this email");
        
        _mockUserRepository.VerifyAll();
    }    
            
    
    [TestMethod]
    public void CreateAdministrator_WhenArgumentsAreValid_ShouldReturnUser(){
        
        var request = new CreateAdministratorRequest{
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "@password1",
        };

        var arguments = new CreateAdministratorArguments(request.FirstName, request.LastName,
            request.Email, request.Password);

        _mockUserRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(false);
        _mockUserRepository.Setup(x => x.Add(It.IsAny<User>()));
        _mockRoleRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Role, bool>>>()));
        
        var result = _service.AddAdministrator(arguments);
        result.Should().NotBeNull();
        result.Email.Should().Be(request.Email);
        _mockUserRepository.VerifyAll();
        _mockRoleRepository.VerifyAll();
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenArgumentsAreValid_ShouldReturnUser()
    {
        var request = new CreateHomeOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "@password1",
            ProfilePicture = "http://www.jpg.com"
        };
        
        var arguments = new CreateHomeOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password, request.ProfilePicture);
        
        _mockUserRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(false);
        _mockUserRepository.Setup(x => x.Add(It.IsAny<User>()));
        _mockRoleRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Role, bool>>>()));
        
        var result = _service.AddHomeOwner(arguments);
        result.Should().NotBeNull();
        result.Email.Should().Be(request.Email);
        _mockUserRepository.VerifyAll();
        _mockRoleRepository.VerifyAll();
    }    

    [TestMethod]
    public void UpdateIfUserIsComplete_ShouldUpdateUser()
    {
        var oldUser = new User()
        {
            IsComplete = false
        };

        var newUser = new User()
        {
            IsComplete = true
        };

        _mockUserRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(oldUser);
        _mockUserRepository.Setup(x => x.Update(It.IsAny<User>()));
           
        var resultUser = _service.UpdateIfUserIsComplete(newUser);
           
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void CreateCompanyOwner_WhenArgumentsAreValid_ShouldReturnUser()
    {

        var request = new CreateCompanyOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "@password1",
        };
        
        var arguments = new CreateCompanyOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password);
        
        _mockUserRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(false);
        _mockUserRepository.Setup(x => x.Add(It.IsAny<User>()));
        _mockRoleRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Role, bool>>>()));
        
        var result = _service.AddCompanyOwner(arguments);
        result.Should().NotBeNull();
        result.Email.Should().Be(request.Email);
        _mockUserRepository.VerifyAll();
        _mockRoleRepository.VerifyAll();
    }
    
    [TestMethod]
    public void CreateHomeOwner_WhenProfilePictureURLIsInvalid_ShouldThrowException()
    {
        var request = new CreateHomeOwnerRequest
        {
            FirstName = "name",
            LastName = "lastname",
            Email = "mail@mail.com",
            Password = "@password1",
            ProfilePicture = "www.jpg"
        };
        
        var arguments = new CreateHomeOwnerArguments(request.FirstName, request.LastName,
            request.Email, request.Password, request.ProfilePicture);
        
        var act = () => _service.AddHomeOwner(arguments);
        
        act.Should().Throw<ArgumentException>().WithMessage("Profile picture must be a valid URL (Parameter 'profilePicture')");
        
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void GetUsers_ShouldReturnUsers()
    {
        List<User> users = new List<User>()
        {
            new User(),
            new User()
        };

        var arguments = new GetAllUsersArguments("Eric", "Administrator", 1, 10);

        _mockUserRepository.Setup(x => x.GetAllWithPagination(It.IsAny<int>(), It.IsAny<int>(),
            It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(users);

        var act = () => _service.GetUsers(arguments);

        act.Should().NotThrow<ServiceException>();
        
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void DeleteAdministratorById_WhenUserDoesNotExist_ShouldThrowException()
    {
        string id = Guid.NewGuid().ToString();
        _mockUserRepository.Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns((User)null);
    
        var act = () => _service.DeleteAdministratorById(id);

        act.Should().NotBeNull();
        act.Should().Throw<EntityNotFoundException>().WithMessage("User not found");
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void DeleteAdministratorById_WhenUserDoesNotHaveRoleAdministrator_ShouldThrowException()
    {
        string id = Guid.NewGuid().ToString();
        var user = new User()
        {
            Id = "B54253y3trrthwrynhr",
            Roles = new List<Role>
            {
                new Role()
                {
                    RoleName = "HomeOwner"
                }
            }
        };

        _mockUserRepository.Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(user);
    
        var act = () => _service.DeleteAdministratorById(id);

        act.Should().NotBeNull();
        act.Should().Throw<ServiceException>().
            WithMessage("Only users with role 'Administrator' can be deleted");
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void DeleteAdministratorById_WhenUserExists_ShouldDeleteUser()
    {
        string id = "B54253y3trrthwrynhr";
        var user = new User()
        {
            Id = id,
            Roles = new List<Role>
            {
                new Role()
                {
                    RoleName = "Administrator"
                }
            }
        };

        _mockUserRepository.Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(user);
        _mockUserRepository.Setup(r => r.Remove(It.Is<User>(u => u.Id == user.Id)));

        var act = () => _service.DeleteAdministratorById(id);

        act.Should().NotThrow<ServiceException>();

        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void GetResidentsByHome_ShouldReturnResidents()
    {
        string homeId = "B54253y3trrthwrynhr";
        List<User> users = new List<User>()
        {
            new User(),
            new User()
        };

        _mockUserRepository.Setup(r => r.GetAll(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);

        var act = () => _service.GetResidentsByHome(homeId);
        act.Should().NotThrow<ServiceException>();
        
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void GetUserByEmail_UserExists_ReturnsUser()
    {
        var expectedUser = new User { Email = "test@example.com" };
        _mockUserRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(expectedUser);
        
        var result = _service.GetUserByEmail("test@example.com");
        
        result.Should().Be(expectedUser);
    }

    [TestMethod]
    public void GetUserByEmail_UserDoesNotExist_ReturnsNull()
    {
        _mockUserRepository.Setup(repo => repo.Add(It.IsAny<User>()));
        
        var result = _service.GetUserByEmail("test@example.com");
        
        result.Should().BeNull();
    }
    
    [TestMethod]
    public void GetUsers_WhenOnlyFullNameAndRoleNameAreNull_ShouldReturnAllUsers()
    {
        var arguments = new GetAllUsersArguments(null, null, 1, 10);
        _mockUserRepository.Setup(repo => repo.GetAllWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(new List<User>());
        
        var act = () => _service.GetUsers(arguments);
        act.Should().NotThrow<ServiceException>();
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void GetUsers_WhenFullNameIsNotNullAndRoleNameIsNull_ShouldReturnUsersWithFullName()
    {
        var arguments = new GetAllUsersArguments("John Doe", null, 1, 10);
        _mockUserRepository.Setup(repo => repo.GetAllWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(new List<User>());
        var act = () => _service.GetUsers(arguments);
        act.Should().NotThrow<ServiceException>();
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void GetUsers_WhenFullNameIsNullAndRoleNameIsNotNull_ShouldReturnUsersWithRoleName()
    {
        var arguments = new GetAllUsersArguments(null, "Administrator", 1, 10);
        _mockUserRepository.Setup(repo => repo.GetAllWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(new List<User>());
        var act = () => _service.GetUsers(arguments);
        act.Should().NotThrow<ServiceException>();
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void GetUsers_WhenFullNameAndRoleNameAreNotNull_ShouldReturnUsersWithFullNameAndRoleName()
    {
        var arguments = new GetAllUsersArguments("John Doe", "Administrator", 1, 10);
        _mockUserRepository.Setup(repo => repo.GetAllWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(new List<User>());
        var act = () => _service.GetUsers(arguments);
        act.Should().NotThrow<ServiceException>();
        
        _mockUserRepository.VerifyAll();
    }
    
    [TestMethod]
    public void User_Id_Should_Get_And_Set_Value()
    {
        var user = new User();
        var id = Guid.NewGuid().ToString();

        user.Id = id;

        user.Id.Should().Be(id);
    }

    [TestMethod]
    public void User_FirstName_Should_Get_And_Set_Value()
    {
        var user = new User();
        var firstName = "John";

        user.FirstName = firstName;

        user.FirstName.Should().Be(firstName);
    }

    [TestMethod]
    public void User_LastName_Should_Get_And_Set_Value()
    {
        var user = new User();
        var lastName = "Doe";

        user.LastName = lastName;

        user.LastName.Should().Be(lastName);
    }

    [TestMethod]
    public void User_Email_Should_Get_And_Set_Value()
    {
        var user = new User();
        var email = "johndoe@example.com";

        user.Email = email;

        user.Email.Should().Be(email);
    }

    [TestMethod]
    public void User_Password_Should_Get_And_Set_Value()
    {
        var user = new User();
        var password = "securePassword123";

        user.Password = password;

        user.Password.Should().Be(password);
    }

    [TestMethod]
    public void User_FullName_Should_Get_And_Set_Value()
    {
        var user = new User();
        var fullName = "John Doe";

        user.FullName = fullName;

        user.FullName.Should().Be(fullName);
    }

    [TestMethod]
    public void User_CreationDate_Should_Get_And_Set_Value()
    {
        var user = new User();
        var creationDate = DateTime.UtcNow;

        user.CreationDate = creationDate;

        user.CreationDate.Should().Be(creationDate);
    }

    [TestMethod]
    public void User_ProfilePicture_Should_Get_And_Set_Value()
    {
        var user = new User();
        var profilePicture = "http://example.com/profile.jpg";

        user.ProfilePicture = profilePicture;

        user.ProfilePicture.Should().Be(profilePicture);
    }

    [TestMethod]
    public void User_IsComplete_Should_Get_And_Set_Value()
    {
        var user = new User();
        var isComplete = true;

        user.IsComplete = isComplete;

        user.IsComplete.Should().Be(isComplete);
    }

    [TestMethod]
    public void User_Residents_Should_Get_And_Set_Value()
    {
        var user = new User();
        var residents = new List<Resident>
        {
            new Resident(),
            new Resident()
        };

        user.Residents = residents;

        user.Residents.Should().BeEquivalentTo(residents);
    }

    [TestMethod]
    public void User_ToString_Should_Return_FullName()
    {
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe"
        };

        var result = user.ToString();

        result.Should().Be("John Doe");
    }
    
    [TestMethod]
    public void HasPermission_UserHasPermission_ReturnsTrue()
    {
        var permissionKey = PermissionKey.ListHomeDevices;
        var user = new User
        {
            Roles = new List<Role>
            {
                new Role
                {
                    SystemPermissions = new List<SystemPermission>
                    {
                        new SystemPermission { Name = permissionKey.ToString() }
                    }
                }
            }
        };
            
        var result = user.HasPermission(permissionKey);
            
        result.Should().BeTrue();
    }
        
    [TestMethod]
    public void SystemPermission_Id_Should_Get_And_Set_Value()
    {
        var systemPermission = new SystemPermission();
        var id = "testId";

        systemPermission.Id = id;

        systemPermission.Id.Should().Be(id);
    }

    [TestMethod]
    public void SystemPermission_Name_Should_Get_And_Set_Value()
    {
        var systemPermission = new SystemPermission();
        var name = "ManageUsers";

        systemPermission.Name = name;

        systemPermission.Name.Should().Be(name);
    }

    [TestMethod]
    public void SystemPermission_Roles_Should_Get_And_Set_Value()
    {
        var systemPermission = new SystemPermission();
        var roles = new List<Role>
        {
            new Role { RoleName = "Administrator" },
            new Role { RoleName = "User" }
        };

        systemPermission.Roles = roles;

        systemPermission.Roles.Should().BeEquivalentTo(roles);
    }

    [TestMethod]
    public void SystemPermission_Should_Initialize_Id()
    {
        var systemPermission = new SystemPermission();

        systemPermission.Id.Should().NotBeNullOrEmpty();
    }
        
        
    [TestMethod]
    public void GetUserById_ShouldReturnUser_WhenUserExists()
    {
        var userId = "testUserId";
        var expectedUser = new User { Id = userId };
        _mockUserRepository.Setup(x => x.Get(
            It.Is<Expression<Func<User, bool>>>(exp => exp.Compile()(expectedUser)),
            It.IsAny<Expression<Func<User, object>>>(),
            It.IsAny<Expression<Func<User, object>>>()
            )).Returns(expectedUser);

        var result = _service.GetUserById(userId);
            
        result.Id.Should().Be(expectedUser.Id);
    }

    [TestMethod]
    public void AddResidentToUser_ShouldAddResidentToUser()
    {
        var user = new User()
        {
            Residents = new List<Resident>()
        };
        var resident = new Resident();
        _mockUserRepository.Setup(repo => repo.Update(It.IsAny<User>()));
        
        _service.AddResidentToUser(user, resident);
        
        user.Residents.Should().Contain(resident);
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void AddHomeOwnerRoleToUser_WhenUserIsNotFound_ShouldThrowException()
    {
        var userId = "testUserId";
        _mockUserRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns((User)null);
        
        var act = () => _service.AddHomeOwnerRoleToUser(userId);
        
        act.Should().Throw<EntityNotFoundException>().WithMessage("User not found");
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void AddHomeOwnerRoleToUser_WhenUserAlreadyHasRoleHomeOwner_ShouldThrowException()
    {
        var userId = "testUserId";
        var user = new User
        {
            Id = userId,
            Roles = new List<Role>
            {
                new Role { RoleName = "HomeOwner" }
            }
        };
        _mockUserRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(user);
        
        var act = () => _service.AddHomeOwnerRoleToUser(userId);
        
        act.Should().Throw<ServiceException>().WithMessage("User already has role 'HomeOwner'");
        _mockUserRepository.VerifyAll();
    }

    [TestMethod]
    public void AddHomeOwnerRoleToUser_WhenChecksArePassed_ShouldAddRoleHomeOwnerToUser()
    {
        var userId = "testUserId";
        var user = new User
        {
            Id = userId,
            Roles = new List<Role>()
        };
        var homeOwnerRole = new Role { RoleName = "HomeOwner" };
        _mockUserRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>>())).Returns(user);
        _mockRoleRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(homeOwnerRole);
        _mockUserRepository.Setup(repo => repo.Update(It.IsAny<User>()));
        
        _service.AddHomeOwnerRoleToUser(userId);
        
        user.Roles.Should().Contain(homeOwnerRole);
        _mockUserRepository.VerifyAll();
        _mockRoleRepository.VerifyAll();
    }
}