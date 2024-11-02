using Domain;
using FluentAssertions;
using smarthome.WebApi.Controllers;
using Moq;
using smarthome.BussinessLogic.Services.HomeServices;
using Domain.HomeModels;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace smarthome.UnitTests.ControllerTests;

[TestClass]
public class HomeControllerTest
{
    private HomeController _controller;
    private Mock<IHomeService> _homeServiceMock;
    private User userLogged;
    
    
    [TestInitialize]
    public void Initialize()
    {
        _homeServiceMock = new Mock<IHomeService>(MockBehavior.Loose);
        _controller = new HomeController(_homeServiceMock.Object);
        userLogged = new User()
        {
            Id = "userLoggedId"
        };
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(hc => hc.Items).Returns(new Dictionary<object, object> { { Items.UserLogged, userLogged } });
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };
    }
    
    [TestMethod]
    public void Create_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.CreateHome(null);
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void Create_WhenRequestIsNotNull_ShouldCallCreateHome()
    {
        var request = new CreateHomeRequest()
        {
            Street = "18 de Julio",
            HouseNumber = 146,
            Latitude = -34.9011,
            Longitude = -56.1882,
            QuantityOfResidents = 5,
            QuantityOfResidentsAllowed = 10,
            Alias = "HomeAlias"
        };

        var expectedHome = new Home()
        {
            Address = new Address(request.Street, request.HouseNumber),
            Location = new Location(request.Latitude, request.Longitude),
            QuantityOfResidents = request.QuantityOfResidents,
            QuantityOfResidentsAllowed = request.QuantityOfResidentsAllowed,
            Owner = userLogged,
            Alias = request.Alias
        };
        _homeServiceMock.Setup(x => x.CreateHome(It.IsAny<CreateHomeArguments>(), It.IsAny<User>()))
                        .Returns(expectedHome);
        var response = _controller.CreateHome(request);
        response.Should().NotBeNull();
        response.Id.Should().Be(expectedHome.Id);
        _homeServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void Create_WhenStreetIsEmpty_ShouldThrowAnException()
    {
        var request = new CreateHomeRequest()
        {
            Street = "",
            HouseNumber = 146,
            Latitude = -34.9011,
            Longitude = -56.1882,
            QuantityOfResidents = 5,
            QuantityOfResidentsAllowed = 10,
        };
        var act = () => _controller.CreateHome(request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'street')");
    }
    
    [TestMethod]
    public void Create_WhenAliasIsEmpty_ShouldThrowAnException()
    {
        var request = new CreateHomeRequest()
        {
            Street = "18 de Julio",
            HouseNumber = 146,
            Latitude = -34.9011,
            Longitude = -56.1882,
            QuantityOfResidents = 5,
            QuantityOfResidentsAllowed = 10,
            Alias = ""
        };
        var act = () => _controller.CreateHome(request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'alias')");
    }
    
    [TestMethod]
    public void AddMember_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.AddUserToHome("1", null);
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void AddMember_WhenRequestIsNotNull_ShouldAddMemberToHome()
    {
        User user = new User()
        {
            Id = "1"
        };
        Home home = new Home()
        {
            Id = "1"
        };
        var request = new AddUserToHomeRequest()
        {
            UserId = user.Id
        };
        var expectedMembership = new Resident(home, new List<HomePermission>());
        var act = () => _controller.AddUserToHome(home.Id, request);
        _homeServiceMock.Setup(x => x.AddUserToHome(home.Id, userLogged.Id, It.IsAny<AddUserToHomeArguments>()))
                        .Returns(expectedMembership);
        var response = _controller.AddUserToHome(home.Id, request);
        response.Should().NotBeNull();
        response.Membership.Home.Should().Be(expectedMembership.Home);
        _homeServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void AddDeviceToHome_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.AddDeviceToHome("1", null);
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void AddDeviceToHome_WhenUserHasPermission_ShouldAddDeviceToHome()
    {
        var request = new AddDeviceToHomeRequest("1");
        var homeId = "1";
        var device = new Device()
        {
            Name = "Device1"
        };
        var permissions = new List<HomePermission>()
        {
            new HomePermission{Name = "BindDeviceToHome"}
        };
        HardwareDevice hardwareDevice = new HardwareDevice(device);
        _homeServiceMock.Setup(x => x.AddDeviceToHome(It.IsAny<AddDeviceToHomeArguments>(), userLogged.Id)).Returns(hardwareDevice);
        var response = _controller.AddDeviceToHome(homeId, request);
        response.Should().NotBeNull();
        response.HardwareDeviceId.Should().Be(hardwareDevice.Id);
        _homeServiceMock.VerifyAll();
    }


    [TestMethod]
    public void GetResidentsByHome_ShouldReturnResidentsByHome()
    {
        var homeId = "testHomeId";
        var getAllResidentsResponse = new List<GetAllResidentsResponse>
        {
            new GetAllResidentsResponse
            {
                FullName = "John Doe",
                Email = "johndoe@example.com",
                ProfilePicture = "profilePicUrl",
                HomePermissions = new List<HomePermission>
                {
                    new HomePermission{Name = "Permission1"},
                    new HomePermission{Name = "Permission2"}
                },
                DoesUserMustBeNotified = true
            }
        };

        _homeServiceMock
            .Setup(x => x.GetResidents(homeId, userLogged.Id))
            .Returns(getAllResidentsResponse);

        var result = _controller.GetResidentsByHome(homeId);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].FullName.Should().Be("John Doe");
        result[0].Email.Should().Be("johndoe@example.com");
        result[0].ProfilePicture.Should().Be("profilePicUrl");
        result[0].HomePermissions.Should().Contain(new List<string> { "Permission1", "Permission2" });
        result[0].DoesUserMustBeNotified.Should().Be(true);

        _homeServiceMock.VerifyAll();
    }

    [TestMethod]
    public void ListHardwareDevicesByHome_ShouldReturnHardwareDevicesByHome()
    {
        var homeId = "testHomeId";
        var getHardwareDevicesDetailInfoResponse = new List<GetHardwareDevicesDetailInfoResponse>
        {
            new GetHardwareDevicesDetailInfoResponse
            {
                DeviceName = "Device1"
            }
        };
        var device = new Device
        {
            Name = "Device1"
        };

        List<HardwareDevice> hardwareDevices = new List<HardwareDevice>
        {
            new HardwareDevice(device)
            {
                ConnectionState = "disconnected"
            }
        };
        
        _homeServiceMock.Setup(x => x.GetHardwareDevicesByHome(It.IsAny<string>(), userLogged.Id, It.IsAny<string>())).Returns(hardwareDevices);

        var result = _controller.ListHardwareDevicesByHome(homeId);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].DeviceName.Should().Be("Device1");

        _homeServiceMock.VerifyAll();
    }

    [TestMethod]
    public void ConfigureResidentsHomePermissions_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.ConfigureResidentsPermissions("1", null);
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void ConfigureResidentsHomePermissions_WhenChecksArePassed_ShouldConfigureResidentsHomePermissions()
    {
        var homeId = "1";
        var request = new ConfigureResidentsHomePermissionsRequest()
        {
            UserId = "1",
            HomePermissionName = "BindDeviceToHome"
        };

        var arguments = new ConfigureResidentsHomePermissionsArguments(request.UserId, request.HomePermissionName);

        var expectedPermissions = new List<HomePermission>()
        {
            new HomePermission{ Name = "BindDeviceToHome" }
        };

        _homeServiceMock.Setup(x => x.ConfigureResidentsPermissions(homeId, It.Is<ConfigureResidentsHomePermissionsArguments>(a => 
            a.UserId == arguments.UserId && a.HomePermission == arguments.HomePermission), userLogged.Id)).Returns(expectedPermissions);

        var act = () => _controller.ConfigureResidentsPermissions(homeId, request);

        act.Should().NotThrow();
        _homeServiceMock.VerifyAll();
    }

    [TestMethod]
    public void ChangeHardwareDeviceConnectionState_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.ChangeHardwareDeviceConnectionState("1", null);
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }

    [TestMethod]
    public void ChangeHardwareDeviceConnectionState_WhenNewConnectionStateIsNull_ShouldThrowAnException()
    {
        var request = new ChangeHardwareDeviceConnectionStateRequest
        {
            NewConnectionState = null
        };

        var act = () => _controller.ChangeHardwareDeviceConnectionState("1", request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'newConnectionState')");
    }

    [TestMethod]
    public void ChangeHardwareDeviceConnectionState_WhenNewConnectionStateIsEmpty_ShouldThrowAnException()
    {
        var request = new ChangeHardwareDeviceConnectionStateRequest
        {
            NewConnectionState = ""
        };

        var act = () => _controller.ChangeHardwareDeviceConnectionState("1", request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'newConnectionState')");
    }

    [TestMethod]
    public void ChangeHardwareDeviceConnectionState_WhenAllChecksArePassed_ShouldUpdateHardwareDeviceConnectionState()
    {
        var hardwareDeviceId = "1";
        var request = new ChangeHardwareDeviceConnectionStateRequest
        {
            NewConnectionState = "connected"
        };
        var device = new Device
        {
            Name = "Device1"
        };

        var hardwareDevice = new HardwareDevice(device);

        _homeServiceMock.Setup(x => x.UpdateHardwareDeviceConnectionState(It.IsAny<string>(),
            It.IsAny<UpdateHardwareDeviceConnectionStateArguments>())).Returns(hardwareDevice);

        var act = () => _controller.ChangeHardwareDeviceConnectionState(hardwareDeviceId, request);

        act.Should().NotThrow();
        _homeServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void ChangeHardwareDeviceName_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.ChangeHardwareDeviceName("1", "2", null);
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void ChangeHardwareDeviceName_WhenRequestIsNotNull_ShouldChangeHardwareDeviceName()
    {
        var homeId = "1";
        var hardwareDeviceId = "2";
        var request = new ChangeHardwareDeviceNameRequest()
        {
            NewName = "NewName"
        };
        var arguments = new ChangeHardwareDeviceNameArguments(request.NewName);
        _homeServiceMock.Setup(x => x.ChangeHardwareDeviceName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ChangeHardwareDeviceNameArguments>(), It.IsAny<string>()));
        var act = () => _controller.ChangeHardwareDeviceName(homeId, hardwareDeviceId, request);
        act.Should().NotThrow();
        _homeServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void ChangeHomeAlias_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.ChangeHomeAlias("1", null);
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void ChangeHomeAlias_WhenAliasIsEmpty_ShouldThrowAnException()
    {
        var request = new ChangeHomeAliasRequest()
        {
            Alias = ""
        };
        var act = () => _controller.ChangeHomeAlias("1", request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'alias')");
    }
    
    [TestMethod]
    public void ChangeHomeAlias_WhenAliasIsNull_ShouldThrowAnException()
    {
        var request = new ChangeHomeAliasRequest()
        {
            Alias = null
        };
        var act = () => _controller.ChangeHomeAlias("1", request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'alias')");
    }
    
    [TestMethod]
    public void ChangeHomeAlias_WhenAllChecksArePassed_ShouldChangeHomeAlias()
    {
        var homeId = "1";
        
        var request = new ChangeHomeAliasRequest()
        {
            Alias = "HomeAlias"
        };
        
        var arguments = new ChangeHomeAliasArguments(request.Alias);
        
        var expectedHome = new Home()
        {
            Alias = request.Alias
        };
        
        _homeServiceMock.Setup(x => x.ChangeHomeAlias(homeId, It.Is<ChangeHomeAliasArguments>(a => a.Alias == arguments.Alias), userLogged)).Returns(expectedHome);
        
        var act = () => _controller.ChangeHomeAlias(homeId, request);
        
        act.Should().NotThrow();
        act.Should().NotBeNull();
        _homeServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void AddRoomToHome_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.AddRoomToHome("1", null);
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void AddRoomToHome_WhenNameIsEmpty_ShouldThrowAnException()
    {
        var request = new AddRoomToHomeRequest("");
        var act = () => _controller.AddRoomToHome("1", request);
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }
    
    [TestMethod]
    public void AddRoomToHome_WhenAllChecksArePassed_ShouldAddRoomToHome()
    {
        var homeId = "1";
        var request = new AddRoomToHomeRequest("RoomName");
        var arguments = new AddRoomToHomeArguments(request.Name, homeId);
        var expectedRoom = new Room()
        {
            Name = request.Name
        };
        _homeServiceMock.Setup(x => x.AddRoomToHome(It.Is<AddRoomToHomeArguments>(a => a.Name == arguments.Name), userLogged.Id)).Returns(expectedRoom);
        var act = () => _controller.AddRoomToHome(homeId, request);
        act.Should().NotThrow();
        act.Should().NotBeNull();
        _homeServiceMock.VerifyAll();
    }
}