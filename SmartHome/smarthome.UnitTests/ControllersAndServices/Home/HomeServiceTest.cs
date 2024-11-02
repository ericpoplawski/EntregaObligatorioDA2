using Domain;
using Domain.Exceptions;
using Domain.HomeModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using smarthome.BussinessLogic.Services.HomeServices;
using smarthome.BussinessLogic.Services.System;
using smarthome.Services.System.BusinessLogic;
using System.Linq.Expressions;

namespace smarthome.UnitTests.ServiceTests;

[TestClass]
public class HomeServiceTest
{
    private HomeService _service;
    private Mock<ISystemService> _systemService;
    private Mock<IDeviceService> _deviceService;
    private Mock<IRepository<Home>> _homeRepository;
    private Mock<IRepository<HardwareDevice>> _hardwareDeviceRepository;
    private Mock<IRepository<HomePermission>> _homePermissionRepository;
    private Mock<IRepository<Resident>> _residentRepository;
    private Mock<IRepository<Room>> _roomRepository;
    private User _userLogged;
    private Home _homeOfUserLogged;
    private Device _exampleDevice;

    [TestInitialize]
    public void Initialize()
    {
        _homeRepository = new Mock<IRepository<Home>>();
        _hardwareDeviceRepository = new Mock<IRepository<HardwareDevice>>();
        _homePermissionRepository = new Mock<IRepository<HomePermission>>();
        _residentRepository = new Mock<IRepository<Resident>>();
        _roomRepository = new Mock<IRepository<Room>>();
        _systemService = new Mock<ISystemService>();
        _deviceService = new Mock<IDeviceService>();
        _service = new HomeService(_homeRepository.Object, _hardwareDeviceRepository.Object,
            _homePermissionRepository.Object,
            _residentRepository.Object, _systemService.Object, _deviceService.Object, _roomRepository.Object);

        _userLogged = new User()
        {
            Residents = new List<Resident>()
        };
        _homeOfUserLogged = new Home()
        {
            Owner = _userLogged,
            OwnerId = _userLogged.Id
        };
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(hc => hc.Items).Returns(new Dictionary<object, object>
            { { Items.UserLogged, _userLogged } });
        _exampleDevice = new Device { Id = "deviceId", Name = "deviceName" };
    }

    [TestMethod]
    public void CreateHome_WhenArgumentsAreValid_ShouldCreateHome()
    {
        var userId = Guid.NewGuid().ToString();
        var homeOwner = new User { Id = userId };

        var arguments = new CreateHomeArguments("18 de Julio", 237, -34.9011, -56.1882, 5, 10, "homeAlias");

        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(homeOwner);

        var result = _service.CreateHome(arguments, homeOwner);

        result.Should().NotBeNull();
        result.Address.Street.Should().Be(arguments.Street);
        result.Address.HouseNumber.Should().Be(arguments.HouseNumber);
        result.Location.Latitude.Should().Be(arguments.Latitude);
        result.Location.Longitude.Should().Be(arguments.Longitude);
        result.QuantityOfResidents.Should().Be(arguments.QuantityOfResidents);
        result.QuantityOfResidentsAllowed.Should().Be(arguments.QuantityOfResidentsAllowed);
        result.OwnerId.Should().Be(homeOwner.Id);

        _systemService.VerifyAll();
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddUserToHome_WhenHomeIsFull_ShouldThrowException()
    {
        var arguments = new AddUserToHomeArguments("userId");
        _homeOfUserLogged.QuantityOfResidents = 5;
        _homeOfUserLogged.QuantityOfResidentsAllowed = 5;
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(_homeOfUserLogged);
        var act = () => _service.AddUserToHome(_homeOfUserLogged.Id, _userLogged.Id, arguments);
        act.Should().Throw<ServiceException>().WithMessage("Home is full");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void GetUserHomePermissions_WhenArgumentsAreNotNull_ShouldReturnPermissions()
    {
        User user = new User()
        {
            Id = "userId"
        };

        Home home = new Home()
        {
            Id = "homeId"
        };

        Resident membership = new Resident(home, new List<HomePermission>());
        user.Residents = new List<Resident> { membership };

        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(user);

        _residentRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Resident, bool>>>(),
                It.IsAny<Expression<Func<Resident, object>>[]>())).Returns(membership);
        List<HomePermission> result = _service.GetUserHomePermissions(home.Id, user.Id);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(membership.HomePermissions);
        _systemService.VerifyAll();
    }

    [TestMethod]
    public void AddDeviceToHome_WhenHomeNotFound_ShouldThrowException()
    {
        var arguments = new AddDeviceToHomeArguments("homeId", "roomId", "deviceId");
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns((Home)null);

        Action act = () => _service.AddDeviceToHome(arguments, _userLogged.Id);

        act.Should().Throw<EntityNotFoundException>().WithMessage("Home not found");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddDeviceToHome_WhenRoomNotFound_ShouldThrowException()
    {
        var arguments = new AddDeviceToHomeArguments(_homeOfUserLogged.Id, "roomId", "deviceId");
        Device device = new Device()
        {
            Id = "deviceId"
        };
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(_homeOfUserLogged);

        _roomRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Room, bool>>>())).Returns((Room)null);

        Action act = () => _service.AddDeviceToHome(arguments, _userLogged.Id);

        act.Should().Throw<EntityNotFoundException>().WithMessage("Room not found");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void AddDeviceToHome_WhenArgumentsAreValid_ShouldAddDeviceToHome()
    {
        var arguments = new AddDeviceToHomeArguments(_homeOfUserLogged.Id, "roomId", "deviceId");
        var room = new Room()
        {
            Id = "roomId"
        };
        Device device = new Device()
        {
            Id = "deviceId"
        };
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(_homeOfUserLogged);

        _roomRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Room, bool>>>())).Returns(room);

        _deviceService.Setup(x => x.GetDeviceById(It.IsAny<string>())).Returns(device);

        _hardwareDeviceRepository.Setup(x => x.Add(It.IsAny<HardwareDevice>()));
        HardwareDevice result = _service.AddDeviceToHome(arguments, _userLogged.Id);
        result.Should().NotBeNull();
        result.Home.Should().NotBeNull();
        result.Home.Should().Be(_homeOfUserLogged);
        result.Device.Should().NotBeNull();
        result.Device.Should().Be(device);
        _homeRepository.VerifyAll();
        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void ConfigureResidentsPermissions_WhenUserNotFound_ShouldThrowException()
    {
        var arguments = new ConfigureResidentsHomePermissionsArguments("tg325g4542", "ListHomeDevices");

        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns((User)null);

        var act = () => _service.ConfigureResidentsPermissions("homeId", arguments, _userLogged.Id);

        act.Should().Throw<EntityNotFoundException>().WithMessage("User not found");

        _systemService.VerifyAll();
    }

    [TestMethod]
    public void ConfigureResidentsPermissions_WhenHomeNotFound_ShouldThrowException()
    {
        var arguments = new ConfigureResidentsHomePermissionsArguments("userId", "ListHomeDevices");
        var user = new User { Id = "userId" };
        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(user);
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns((Home)null);

        Action act = () => _service.ConfigureResidentsPermissions("homeId", arguments, _userLogged.Id);

        act.Should().Throw<EntityNotFoundException>().WithMessage("Home not found");
        _systemService.VerifyAll();
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void ConfigureResidentsPermissions_WhenUserIsNotTheOwnerOfTheHome_ShouldThrowException()
    {
        var arguments = new ConfigureResidentsHomePermissionsArguments("userId", "ListHomeDevices");
        var user = new User { Id = "userId" };
        var home = new Home { Owner = new User { Id = "anotherUserId" } };
        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(user);
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);

        Action act = () => _service.ConfigureResidentsPermissions("homeId", arguments, _userLogged.Id);

        act.Should().Throw<ForbiddenException>().WithMessage("User is not the owner of this home");
        _systemService.VerifyAll();
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void ConfigureResidentsPermissions_ValidationsArePassed_ShouldUpdatePermissionsList()
    {
        User user = new User()
        {
            Id = "userId",
            Residents = new List<Resident>()
        };
        var arguments = new ConfigureResidentsHomePermissionsArguments("userId", "ListHomeDevices");

        var resident = new Resident(_homeOfUserLogged, new List<HomePermission>());
        _userLogged.Residents = new List<Resident> { resident };

        var homePermission = new HomePermission { Name = arguments.HomePermission };

        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(user);

        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(_homeOfUserLogged);
        _homePermissionRepository.Setup(x => x.Get(It.IsAny<Expression<Func<HomePermission, bool>>>()))
            .Returns(homePermission);
        _residentRepository.Setup(x => x.Update(It.IsAny<Resident>()));
        _residentRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Resident, bool>>>(),
            It.IsAny<Expression<Func<Resident, object>>[]>())).Returns(resident);

        var result = _service.ConfigureResidentsPermissions(_homeOfUserLogged.Id, arguments, _userLogged.Id);

        result.Should().NotBeNull();
        result.Should().Contain(homePermission);
        _systemService.VerifyAll();
        _homePermissionRepository.VerifyAll();
        _residentRepository.Verify(x => x.Update(resident), Times.Once);
    }

    [TestMethod]
    public void GetHardwareDeviceById_WhenIdNotMatchAnyDevice_ShouldThrowException()
    {
        _hardwareDeviceRepository.Setup(x => x.Get(It.IsAny<Expression<Func<HardwareDevice, bool>>>()))
            .Returns((HardwareDevice)null);

        var act = () => _service.GetHardwareById("hardwareDeviceId");

        act.Should().Throw<EntityNotFoundException>().WithMessage("Hardware device not found");

        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void GetHardwareDeviceById_WhenIdMatchDevice_ShouldReturnDevice()
    {
        var hardwareDevice = new HardwareDevice(_exampleDevice)
        {
            ConnectionState = "connected"
        };
        _hardwareDeviceRepository.Setup(x => x.Get(It.IsAny<Expression<Func<HardwareDevice, bool>>>()))
            .Returns(hardwareDevice);

        var result = _service.GetHardwareById("hardwareDeviceId");

        result.Should().NotBeNull();
        result.ConnectionState.Should().Be(hardwareDevice.ConnectionState);

        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void GetHomeById_WhenIdNotMatchAnyHome_ShouldThrowException()
    {
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns((Home)null);

        var act = () => _service.GetHomeById("homeId");

        act.Should().Throw<EntityNotFoundException>().WithMessage("Home not found");

        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void GetHomeById_WhenIdMatchHome_ShouldReturnHome()
    {
        var home = new Home();
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);

        var result = _service.GetHomeById("homeId");
        result.Should().NotBeNull();
        result.Should().Be(home);
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void GetResidents_WhenHomeNotFound_ShouldThrowException()
    {
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns((Home)null);

        var act = () => _service.GetResidents("homeId", _userLogged.Id);

        act.Should().Throw<EntityNotFoundException>().WithMessage("Home not found");

        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void GetResidents_ShouldReturnResidents()
    {
        var users = new List<User>
        {
            new User
            {
                FullName = "Jane Doe",
                Email = "janedoe@example.com",
                ProfilePicture = "profilePicUrl",
                Residents = new List<Resident>
                {
                    new Resident
                    {
                        Home = _homeOfUserLogged,
                        HomePermissions = new List<HomePermission>
                        {
                            new HomePermission
                            {
                                Name = PermissionKey.CanReceiveNotifications.ToString()
                            }
                        }
                    }
                }
            }
        };

        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(_homeOfUserLogged);

        _systemService
            .Setup(x => x.GetResidentsByHome(_homeOfUserLogged.Id))
            .Returns(users);


        var result = _service.GetResidents(_homeOfUserLogged.Id, _userLogged.Id);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        var resident = result[0];
        resident.FullName.Should().Be("Jane Doe");
        resident.Email.Should().Be("janedoe@example.com");
        resident.ProfilePicture.Should().Be("profilePicUrl");
        resident.HomePermissions.Should().HaveCount(1);
        resident.HomePermissions[0].Name.Should().Be(PermissionKey.CanReceiveNotifications.ToString());
        resident.DoesUserMustBeNotified.Should().BeTrue();

        _homeRepository.VerifyAll();
        _systemService.VerifyAll();
    }

    [TestMethod]
    public void GetHardwareDevicesByHome_WhenHomeNotFound_ShouldThrowException()
    {
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns((Home)null);

        var act = () => _service.GetHardwareDevicesByHome("homeId", _userLogged.Id);

        act.Should().Throw<EntityNotFoundException>().WithMessage("Home not found");

        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void GetHardwareDevicesByHome_ShouldReturnHardwareDevices()
    {
        var hardwareDevices = new List<HardwareDevice>
        {
            new HardwareDevice(_exampleDevice)
            {
                ConnectionState = "disconnected"
            }
        };

        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>()))
            .Returns(_homeOfUserLogged);

        _hardwareDeviceRepository
            .Setup(x => x.GetAll(It.IsAny<Expression<Func<HardwareDevice, bool>>>()))
            .Returns(hardwareDevices);

        var result = _service.GetHardwareDevicesByHome(_homeOfUserLogged.Id, _userLogged.Id);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Device.Name.Should().Be("deviceName");

        _homeRepository.VerifyAll();
        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void UpdateHardwareDeviceConnectionState_WhenHardwareDeviceNotFound_ShouldThrowException()
    {
        _hardwareDeviceRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<HardwareDevice, bool>>>()));

        var act = () => _service.UpdateHardwareDeviceConnectionState
            ("hardwareDeviceId", new UpdateHardwareDeviceConnectionStateArguments("connected"));

        act.Should().Throw<EntityNotFoundException>().WithMessage("Hardware device not found");

        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void UpdateHardwareDeviceConnectionState_WhenNewConnectionStateIsInvalid_ShouldThrowException()
    {
        var hardwareDevice = new HardwareDevice(_exampleDevice)
        {
            ConnectionState = "connected"
        };

        _hardwareDeviceRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<HardwareDevice, bool>>>()))
            .Returns(hardwareDevice);

        var act = () => _service.UpdateHardwareDeviceConnectionState
            ("hardwareDeviceId", new UpdateHardwareDeviceConnectionStateArguments("invalid"));

        act.Should().Throw<ServiceException>().WithMessage("Connection state must only be connected or disconnected");

        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void UpdateHardwareDeviceConnectionState_WhenNewConnectionStateIsEqualToActualState_ShouldThrowException()
    {
        var hardwareDevice = new HardwareDevice(_exampleDevice)
        {
            ConnectionState = "connected"
        };

        _hardwareDeviceRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<HardwareDevice, bool>>>()))
            .Returns(hardwareDevice);

        var act = () => _service.UpdateHardwareDeviceConnectionState
            ("hardwareDeviceId", new UpdateHardwareDeviceConnectionStateArguments("connected"));

        act.Should().Throw<ServiceException>().WithMessage("New connection state must differ from actual state");

        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void UpdateHardwareDeviceConnectionState_WhenArgumentsAreValid_ShouldUpdateConnectionState()
    {
        var hardwareDevice = new HardwareDevice(_exampleDevice)
        {
            ConnectionState = "connected"
        };

        _hardwareDeviceRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<HardwareDevice, bool>>>()))
            .Returns(hardwareDevice);

        _hardwareDeviceRepository
            .Setup(x => x.Update(It.IsAny<HardwareDevice>()));

        _service.UpdateHardwareDeviceConnectionState
            ("hardwareDeviceId", new UpdateHardwareDeviceConnectionStateArguments("disconnected"));

        hardwareDevice.ConnectionState.Should().Be("disconnected");

        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void GetResidents_WhenUserIsNotTheOwnerOfTheHome_ShouldThrowException()
    {
        var homeId = "homeId";
        var userId = "userId";
        var home = new Home
        {
            Id = homeId,
            Owner = new User()
        };
        _homeRepository.Setup(x => x.Get(It.Is<Expression<Func<Home, bool>>>(expr => expr.Compile()(home))))
            .Returns(home);

        Action act = () => _service.GetResidents(homeId, userId);

        act.Should().Throw<ForbiddenException>().WithMessage("User is not the owner of this home");
        _homeRepository.VerifyAll();
    }

    [TestMethod]
    public void GetUserHomePermissions_UserIsNotResident_ThrowsServiceException()
    {
        var userId = "testUserId";
        var homeId = "testHomeId";

        var user = new User
        {
            Id = userId,
            Residents = null
        };

        var home = new Home
        {
            Id = homeId
        };

        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(user);

        Action act = () => _service.GetUserHomePermissions(homeId, userId);

        act.Should().Throw<ServiceException>().WithMessage("User is not a resident of this home");
        _systemService.VerifyAll();
    }

    [TestMethod]
    public void CreateHome_QuantityOfResidentsGreaterThanAllowed_ThrowsServiceException()
    {
        var arguments = new CreateHomeArguments("18 de Julio", 237, -34.9011, -56.1882, 5, 4, "homeAlias");
        var homeOwner = new User { Id = "userId" };

        Action act = () => _service.CreateHome(arguments, homeOwner);

        act.Should().Throw<ServiceException>()
            .WithMessage("Quantity of residents cannot be greater than quantity of residents allowed");
    }


    [TestMethod]
    public void AddUserToHome_UserIsNotOwner_ThrowsServiceException()
    {
        var home = new Home { Owner = new User { Id = "1" } };
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(home);

        Action act = () => _service.AddUserToHome("homeId", "2", new AddUserToHomeArguments("1"));

        act.Should().Throw<ForbiddenException>().WithMessage("User is not the owner of this home");
    }

    [TestMethod]
    public void AddUserToHome_HomeIsFull_ThrowsServiceException()
    {
        var home = new Home { Owner = new User { Id = "1" }, QuantityOfResidents = 5, QuantityOfResidentsAllowed = 5 };
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(home);

        Action act = () => _service.AddUserToHome("homeId", "1", new AddUserToHomeArguments("2"));

        act.Should().Throw<ServiceException>().WithMessage("Home is full");
    }

    [TestMethod]
    public void AddUserToHome_UserIsResidentOfAnotherHome_DoesNotThrowException()
    {
        var home = new Home
            { Id = "homeId", Owner = new User { Id = "1" }, QuantityOfResidents = 4, QuantityOfResidentsAllowed = 5 };
        var anotherHome = new Home
        {
            Id = "anotherHomeId", Owner = new User { Id = "1" }, QuantityOfResidents = 4, QuantityOfResidentsAllowed = 5
        };
        var user = new User
        {
            Id = "2", Roles = new List<Role> { new Role { RoleName = "HomeOwner" } },
            Residents = new List<Resident> { new Resident(anotherHome, new List<HomePermission>()) }
        };
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);
        _homeRepository.Setup(x => x.Update(home));

        Action act = () => _service.AddUserToHome("homeId", "1", new AddUserToHomeArguments("2"));

        act.Should().NotThrow<ServiceException>();
    }

    [TestMethod]
    public void Home_Constructor_Should_Initialize_Properties_Correctly()
    {
        var address = new Address("18 de Julio", 237);
        var location = new Location(-34.9011, -56.1882);
        var quantityOfResidents = 3;
        var quantityOfResidentsAllowed = 5;
        var owner = new User { Id = "ownerId" };
        var member = new User { Id = "memberId" };

        var home = new Home(address, location, quantityOfResidents, quantityOfResidentsAllowed, owner, member,
            "homeAlias");

        home.Id.Should().NotBeNullOrEmpty();
        home.Address.Should().Be(address);
        home.Location.Should().Be(location);
        home.QuantityOfResidents.Should().Be(quantityOfResidents);
        home.QuantityOfResidentsAllowed.Should().Be(quantityOfResidentsAllowed);
        home.Owner.Should().Be(owner);
        home.OwnerId.Should().Be(owner.Id);
    }

    [TestMethod]
    public void AddDeviceToHome_WhenDeviceTypeIsWindowSensor_ShouldCreateHardwareDeviceWithOpeningStateClosed()
    {
        var home = new Home { Owner = _userLogged };
        var device = new Device { Id = "testDeviceId", Type = "windowSensor" };
        var room = new Room { Id = "testRoomId" };
        var arguments = new AddDeviceToHomeArguments(home.Id, room.Id, device.Id);

        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(home);

        _roomRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Room, bool>>>())).Returns(room);

        _deviceService.Setup(x => x.GetDeviceById(It.IsAny<string>())).Returns(device);

        var result = _service.AddDeviceToHome(arguments, _userLogged.Id);

        result.OpeningState.Should().Be("closed");
        result.ConnectionState.Should().Be("connected");
        result.Device.Should().Be(device);
        result.Home.Should().Be(home);
    }

    [TestMethod]
    public void GetHardwareById_WhenIncludesIsTrue_ShouldReturnHardwareDeviceWithHomeAndDevice()
    {
        var hardwareDeviceId = "testId";
        var expectedHardwareDevice = new HardwareDevice(_exampleDevice)
        {
            Id = hardwareDeviceId,
            Home = new Home(),
            Device = new Device()
        };

        _hardwareDeviceRepository.Setup(repo => repo.Get(
                It.IsAny<Expression<Func<HardwareDevice, bool>>>(),
                It.IsAny<Expression<Func<HardwareDevice, object>>[]>()))
            .Returns(expectedHardwareDevice);

        var result = _service.GetHardwareById(hardwareDeviceId, true);

        result.Should().Be(expectedHardwareDevice);
        result.Home.Should().NotBeNull();
        result.Device.Should().NotBeNull();
    }

    [TestMethod]
    public void GetResidentById_ShouldReturnResident()
    {
        var residentId = "testResidentId";
        var expectedResident = new Resident { Id = residentId };

        _residentRepository.Setup(_residentRepository => _residentRepository.Get(
            It.IsAny<Expression<Func<Resident, bool>>>(),
            It.IsAny<Expression<Func<Resident, object>>[]>())).Returns(expectedResident);

        var result = _service.GetResidentById(residentId);
        result.Should().Be(expectedResident);
    }

    [TestMethod]
    public void ChangeHardwareDeviceName_WhenUserIsNotAResidentOfHome_ShouldReturnForbiddenException()
    {
        var homeId = "testHomeId";
        var hardwareDeviceId = "testHardwareDeviceId";
        var arguments = new ChangeHardwareDeviceNameArguments("newName");
        var userLoggedId = "testUserLoggedId";
        var home = new Home { Id = homeId, Owner = new User { Id = "anotherUserId" } };
        var device = new Device { Id = "testDeviceId", Name = "deviceName" };
        var hardwareDevice = new HardwareDevice(device) { Id = hardwareDeviceId, Home = home };
        List<Resident> residents = new List<Resident>();

        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);
        _systemService.Setup(x => x.GetUserById(It.IsAny<string>()))
            .Returns(new User { Id = userLoggedId, Residents = new List<Resident>() });

        Action act = () => _service.ChangeHardwareDeviceName(homeId, hardwareDeviceId, arguments, userLoggedId);

        act.Should().Throw<ForbiddenException>().WithMessage("User is not a resident of this home");
    }

    [TestMethod]
    public void ChangeHardwareDeviceName_WhenUserDoesNotHavePermission_ShouldReturnForbiddenException()
    {
        var homeId = "testHomeId";
        var hardwareDeviceId = "testHardwareDeviceId";
        var arguments = new ChangeHardwareDeviceNameArguments("newName");
        var userLoggedId = "testUserLoggedId";
        var home = new Home { Id = homeId, Owner = new User { Id = "anotherUserId" } };
        var device = new Device { Id = "testDeviceId", Name = "deviceName" };
        var hardwareDevice = new HardwareDevice(device) { Id = hardwareDeviceId, Home = home };
        List<Resident> residents = new List<Resident> { new Resident(home, new List<HomePermission>()) };

        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(home);
        _systemService.Setup(x => x.GetUserById(It.IsAny<string>()))
            .Returns(new User { Id = userLoggedId, Residents = residents });
        _residentRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Resident, bool>>>(),
                It.IsAny<Expression<Func<Resident, object>>[]>())).Returns(residents[0]);

        Action act = () => _service.ChangeHardwareDeviceName(hardwareDeviceId, homeId, arguments, userLoggedId);

        act.Should().Throw<ForbiddenException>()
            .WithMessage("User does not have permission to change hardware device name");
    }

    [TestMethod]
    public void ChangeHardwareDeviceName_WhenUserIsOwner_ShouldChangeHardwareDeviceName()
    {
        var homeId = "testHomeId";
        var hardwareDeviceId = "testHardwareDeviceId";
        var arguments = new ChangeHardwareDeviceNameArguments("newName");
        var userLoggedId = "testUserLoggedId";
        var home = new Home { Id = homeId, Owner = new User { Id = userLoggedId } };
        var device = new Device { Id = "testDeviceId", Name = "deviceName" };
        var hardwareDevice = new HardwareDevice(device) { Id = hardwareDeviceId, Home = home };
        List<Resident> residents = new List<Resident> { new Resident(home, new List<HomePermission>()) };

        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(home);
        _systemService.Setup(x => x.GetUserById(It.IsAny<string>()))
            .Returns(new User { Id = userLoggedId, Residents = residents });
        _residentRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Resident, bool>>>(),
                It.IsAny<Expression<Func<Resident, object>>[]>())).Returns(residents[0]);
        _hardwareDeviceRepository.Setup(x => x.Get(It.IsAny<Expression<Func<HardwareDevice, bool>>>()))
            .Returns(hardwareDevice);
        _hardwareDeviceRepository.Setup(x => x.Update(It.IsAny<HardwareDevice>()));

        _service.ChangeHardwareDeviceName(hardwareDeviceId, homeId, arguments, userLoggedId);

        hardwareDevice.Name.Should().Be("newName");
        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void ChangeHardwareDeviceName_WhenUserHasPermission_ShouldChangeHardwareDeviceName()
    {
        var homeId = "testHomeId";
        var hardwareDeviceId = "testHardwareDeviceId";
        var arguments = new ChangeHardwareDeviceNameArguments("newName");
        var userLoggedId = "testUserLoggedId";
        var home = new Home { Id = homeId, Owner = new User { Id = "anotherUserId" } };
        var device = new Device { Id = "testDeviceId", Name = "deviceName" };
        var hardwareDevice = new HardwareDevice(device) { Id = hardwareDeviceId, Home = home };
        List<Resident> residents = new List<Resident>
        {
            new Resident(home, new List<HomePermission> { new HomePermission() { Name = "ChangeHardwareDeviceName" } })
        };
        _homeRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>>()))
            .Returns(home);
        _systemService.Setup(x => x.GetUserById(It.IsAny<string>()))
            .Returns(new User { Id = userLoggedId, Residents = residents });
        _residentRepository
            .Setup(x => x.Get(It.IsAny<Expression<Func<Resident, bool>>>(),
                It.IsAny<Expression<Func<Resident, object>>[]>())).Returns(residents[0]);
        _hardwareDeviceRepository.Setup(x => x.Get(It.IsAny<Expression<Func<HardwareDevice, bool>>>()))
            .Returns(hardwareDevice);
        _hardwareDeviceRepository.Setup(x => x.Update(It.IsAny<HardwareDevice>()));

        _service.ChangeHardwareDeviceName(hardwareDeviceId, homeId, arguments, userLoggedId);

        hardwareDevice.Name.Should().Be("newName");
        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void ChangeHomeAlias_WhenHomeNotFound_ShouldThrowEntityNotFoundException()
    {
        var homeId = "homeId";
        var userId = "userId";
        var arguments = new ChangeHomeAliasArguments("newAlias");
        var user = new User { Id = userId };
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns((Home)null);

        Action act = () => _service.ChangeHomeAlias(homeId, arguments, user);

        act.Should().Throw<EntityNotFoundException>().WithMessage("Home not found");
    }

    [TestMethod]
    public void ChangeHomeAlias_WhenArgumentsAreValid_ShouldChangeAlias()
    {
        var homeId = "homeId";
        var userId = "userId";
        var arguments = new ChangeHomeAliasArguments("newAlias");
        var home = new Home { Owner = new User { Id = userId }, OwnerId = userId };
        var user = new User { Id = userId, Residents = new List<Resident>() };
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);
        _homeRepository.Setup(x => x.Update(home));

        _service.ChangeHomeAlias(homeId, arguments, user);

        home.Alias.Should().Be(arguments.Alias);
    }

    [TestMethod]
    public void ChangeHomeAlias_WhenUserIsNotTheOwnerOfTheHome_ShouldThrowForbiddenException()
    {
        var homeId = "homeId";
        var userId = "userId";
        var arguments = new ChangeHomeAliasArguments("newAlias");
        var home = new Home { Owner = new User { Id = "anotherUserId" } };
        var user = new User { Id = userId, Residents = new List<Resident>() };
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);

        Action act = () => _service.ChangeHomeAlias(homeId, arguments, user);

        act.Should().Throw<ForbiddenException>().WithMessage("User is not the owner or resident of this home");
    }

    [TestMethod]
    public void ChangeHomeAlias_WhenUserIsNotResidentOfTheHome_ShouldThrowForbiddenException()
    {
        var homeId = "homeId";
        var ownerId = "ownerId";
        var userId = "userId";
        var arguments = new ChangeHomeAliasArguments("newAlias");
        var home = new Home { Owner = new User { Id = ownerId }, OwnerId = ownerId };
        var user = new User { Id = userId, Residents = new List<Resident>() };
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);
        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(user);
        _residentRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Resident, bool>>>())).Returns((Resident)null);

        Action act = () => _service.ChangeHomeAlias(homeId, arguments, user);

        act.Should().Throw<ForbiddenException>().WithMessage("User is not the owner or resident of this home");
    }

    [TestMethod]
    public void AddRoomToHome_WhenHomeIsNotFound_ShouldThrowEntityNotFoundException()
    {
        var homeId = "homeId";
        var request = new AddRoomToHomeRequest("roomName");
        var arguments = new AddRoomToHomeArguments(request.Name, homeId);
        var userLogged = new User { Id = "userId" };
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns((Home)null);

        Action act = () => _service.AddRoomToHome(arguments, userLogged.Id);

        act.Should().Throw<EntityNotFoundException>().WithMessage("Home not found");
    }


    [TestMethod]
    public void AddRoomToHome_UserWithoutPermission_ShouldThrowForbiddenException()
    {
        var homeId = "homeId";
        var userLoggedId = "userLoggedId";
        var arguments = new AddRoomToHomeArguments("roomName", homeId);

        var home = new Home { Id = homeId, OwnerId = "anotherUserId" };
        _homeRepository.Setup(repo =>
                repo.Get(It.IsAny<Expression<Func<Home, bool>>>(), It.IsAny<Expression<Func<Home, object>>[]>()))
            .Returns(home);

        var permissions = new List<HomePermission>();
        var resident = new Resident(home, permissions);
        _residentRepository.Setup(repo => repo.Get(It.IsAny<Expression<Func<Resident, bool>>>(),
                It.IsAny<Expression<Func<Resident, object>>[]>()))
            .Returns(resident);

        var user = new User { Id = userLoggedId, Residents = new List<Resident> { resident } };
        _systemService.Setup(s => s.GetUserById(userLoggedId)).Returns(user);

        Action act = () => _service.AddRoomToHome(arguments, userLoggedId);

        act.Should().Throw<ForbiddenException>()
            .WithMessage("User does not have permission to add a room to this home");
    }


    [TestMethod]
    public void AddRoomToHome_WhenRoomWithSameNameExists_ShouldThrowServiceException()
    {
        var arguments = new AddRoomToHomeArguments(_homeOfUserLogged.Id, "existingRoomName");
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(_homeOfUserLogged);
        _roomRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Room, bool>>>())).Returns(true);

        Action act = () => _service.AddRoomToHome(arguments, _userLogged.Id);
        act.Should().Throw<ServiceException>().WithMessage("A room with the same name already exists in this home");

        _homeRepository.VerifyAll();
        _roomRepository.VerifyAll();
    }

    [TestMethod]
    public void AddRoomToHome_WhenArgumentsAreValid_ShouldAddRoomToHome()
    {
        var arguments = new AddRoomToHomeArguments(_homeOfUserLogged.Id, "roomName");
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(_homeOfUserLogged);
        _roomRepository.Setup(x => x.Add(It.IsAny<Room>())).Callback<Room>(room => room.Name = arguments.Name);

        var result = _service.AddRoomToHome(arguments, _userLogged.Id);

        result.Should().NotBeNull();
        result.Name.Should().Be(arguments.Name);
        result.Home.Should().Be(_homeOfUserLogged);
        _homeRepository.VerifyAll();
        _roomRepository.VerifyAll();
    }

    [TestMethod]
    public void UpdateHardwareDevice_ShouldUpdateHardwareDevice()
    {
        var hardwareDevice = new HardwareDevice()
        {
            Id = "hardwareDeviceId",
            PowerState = "off"
        };

        _hardwareDeviceRepository.Setup(x => x.Update(It.IsAny<HardwareDevice>()));

        var act = () => _service.UpdateHardwareDevice(hardwareDevice);

        act.Should().NotThrow<Exception>();

        _hardwareDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void DoesUserHaveNotificationPermissionInSpecificHome_WhenUserIsNotResidentOfHome_ShouldReturnFalse()
    {
        var homeId = "homeId";
        var userId = "userId";
        var user = new User { Id = userId };
        _systemService.Setup(x => x.GetUserById(It.IsAny<string>())).Returns(user);
        var home = new Home { Id = homeId, Owner = new User { Id = "anotherUserId" } };
        _homeRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Home, bool>>>())).Returns(home);
        
        var result = _service.DoesUserHaveNotificationPermissionInSpecificHome(userId, homeId);

        result.Should().BeFalse();
    }
}