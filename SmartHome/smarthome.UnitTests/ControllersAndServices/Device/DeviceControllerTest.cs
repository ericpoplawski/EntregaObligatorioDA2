using Domain.DeviceModels;
using Domain.Exceptions;
using FluentAssertions;
using Moq;
using smarthome.BussinessLogic.Services.System;
using smarthome.WebApi.Controllers;
using Domain;
using Domain.DeviceModels.RegisterSmartLamp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.DeviceModels.ImportDevice;

namespace smarthome.UnitTests;

[TestClass]
public sealed class DeviceControllerTest
{
    private DeviceController _controller;
    private Mock<IDeviceService> _deviceServiceMock;
    private Mock<HttpContext> _httpContextMock;

    [TestInitialize]
    public void Initialize()
    {
        _deviceServiceMock = new Mock<IDeviceService>(MockBehavior.Strict);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        ControllerContext controllerContext = new ControllerContext
        {
            HttpContext = _httpContextMock.Object
        };
        _controller = new DeviceController(_deviceServiceMock.Object);
        _controller.ControllerContext = controllerContext;

        User user = new User()
        {
            Roles = new List<Role>()
            {
                new Role
                {
                    RoleName = RoleKey.CompanyOwner.ToString(),
                    SystemPermissions = new List<SystemPermission>()
                    {
                        new SystemPermission
                        {
                             Name = PermissionKey.RegisterSecurityCamera.ToString()
                        },
                        new SystemPermission
                        {
                             Name = PermissionKey.RegisterWindowSensor.ToString()
                        }
                    }
                }
            }
        };
        _httpContextMock.SetupGet(c => c.Items[Items.UserLogged]).Returns(user);
    }
    
    [TestMethod]
    public void RegisterSecurityCamera_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.RegisterSecurityCamera(null);
        
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void RegisterWindowSensor_WhenRequestIsNull_ShouldThrowAnException()
    {
        var act = () => _controller.RegisterWindowSensor(null);
        
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void RegisterSecurityCamera_WhenNameIsEmpty_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest
        {
            Name = "",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>(),
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };


        var act = () => _controller.RegisterSecurityCamera(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }
    
    [TestMethod]
    public void RegisterWindowSensor_WhenNameIsEmpty_ShouldThrowException()
    {
        var request = new RegisterWindowSensorRequest
        {
            Name = "",
        };
        
        var act = () => _controller.RegisterWindowSensor(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }
    
    [TestMethod]
    public void RegisterSecurityCamera_WhenNameIsNull_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest
        {
            Name = null,
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>(),
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };
        
        var act = () => _controller.RegisterSecurityCamera(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }
    
    [TestMethod]
    public void RegisterWindowSensor_WhenNameIsNull_ShouldThrowException()
    {
        var request = new RegisterWindowSensorRequest
        {
            Name = null,
        };
        
        var act = () => _controller.RegisterWindowSensor(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }

    [TestMethod]
    public void RegisterSecurityCamera_WhenModelNumberIsNull_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = null,
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>(),
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };
        
        var act = () => _controller.RegisterSecurityCamera(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'modelNumber')");
    }
    
    [TestMethod]
    public void RegisterWindowSensor_WhenModelNumberIsNull_ShouldThrowException()
    {
        var request = new RegisterWindowSensorRequest()
        {
            Name = "name",
            ModelNumber = null,
        };
        
        var act = () => _controller.RegisterWindowSensor(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'modelNumber')");
    }
    
    [TestMethod]
    public void RegisterSecurityCamera_WhenModelNumberIsEmpty_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = "",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>(),
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };

        var act = () => _controller.RegisterSecurityCamera(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'modelNumber')");
    }
    
    [TestMethod]
    public void RegisterWindowSensor_WhenModelNumberIsEmpty_ShouldThrowException()
    {
        var request = new RegisterWindowSensorRequest()
        {
            Name = "name",
            ModelNumber = "",
        };
        
        var act = () => _controller.RegisterWindowSensor(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'modelNumber')");
    }
    
    [TestMethod]
    public void RegisterSecurityCamera_WhenMainPictureIsEmpty_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "",
            Photographies = new List<string>(),
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };

        var act = () => _controller.RegisterSecurityCamera(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'mainPicture')");
    }
    
    [TestMethod]
    public void RegisterSecurityCamera_WhenMainPictureIsNull_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = null,
            Photographies = new List<string>(),
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };
        
        var act = () => _controller.RegisterSecurityCamera(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'mainPicture')");
    }
    
    [TestMethod]
    public void RegisterSecurityCamera_WhenUsageTypeIsEmpty_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>(),
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = ""
        };
        
        var act = () => _controller.RegisterSecurityCamera(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'usageType')");
    }
    
    [TestMethod]
    public void RegisterSecurityCamera_WhenUsageTypeIsNull_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = null,
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = null
        };
        
        var act = () => _controller.RegisterSecurityCamera(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'usageType')");
    }

    [TestMethod]
    public void RegisterSecurityCamera_WhenRequestIsValid_ShouldRegisterDevice()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "Camera",
            ModelNumber = "Model",
            Description = "Description",
            MainPicture = "MainPicture",
            Photographies = new List<string>(),
            UsageType = "Indoor"
        };

        var expectedCamera = new Device()
        {
            Name = request.Name,
            ModelNumber = request.ModelNumber,
            Description = request.Description,
            MainPicture = request.MainPicture,
            Photographies = request.Photographies,
            UsageType = request.UsageType
        };

        var act = () => _controller.RegisterSecurityCamera(request);
        _deviceServiceMock.Setup(x => x.RegisterSecurityCamera(It.IsAny<RegisterSecurityCameraArguments>(), It.IsAny<User>())).Returns(expectedCamera);
        var response = _controller.RegisterSecurityCamera(request);

        response.Should().NotBeNull();
        response.Id.Should().Be(expectedCamera.Id);
        _deviceServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void RegisterWindowSensor_WhenRequestIsValid_ShouldRegisterDevice()
    {
        var request = new RegisterWindowSensorRequest()
        {
            Name = "WindowSensor",
            ModelNumber = "Model",
            Description = "Description",
            Photographies = new List<string>()
        };

        var expectedWindowSensor = new Device()
        {
            Name = request.Name,
            ModelNumber = request.ModelNumber,
            Description = request.Description,
            Photographies = request.Photographies
        };

        var act = () => _controller.RegisterWindowSensor(request);
        _deviceServiceMock.Setup(x => x.RegisterWindowSensor
        (It.IsAny<RegisterWindowSensorArguments>(), It.IsAny<User>())).Returns(expectedWindowSensor);
        var response = _controller.RegisterWindowSensor(request);

        response.Should().NotBeNull();
        response.Id.Should().Be(expectedWindowSensor.Id);
        _deviceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenRequestIsNull_ShouldThrowException()
    {
        var act = () => _controller.RegisterMotionSensor(null);
        
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenNameIsNull_ShouldThrowException()
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = null
        };

        var act = () => _controller.RegisterMotionSensor(request);
        
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenNameIsEmpty_ShouldThrowException()
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = ""
        };

        var act = () => _controller.RegisterMotionSensor(request);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenModelNumberIsNull_ShouldThrowException()
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = "Samsung Aventador",
            ModelNumber = null
        };

        var act = () => _controller.RegisterMotionSensor(request);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'modelNumber')");
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenModelNumberIsEmpty_ShouldThrowException()
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = "Samsung aventador",
            ModelNumber = ""
        };

        var act = () => _controller.RegisterMotionSensor(request);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'modelNumber')");
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenAllChecksArePassed_ShouldRegisterDevice() 
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = "Samsung Aventador",
            ModelNumber = "CC234P",
            Description = "This is a motion sensor"
        };

        var expectedMotionSensor = new Device
        {
            Name = request.Name,
            ModelNumber = request.ModelNumber,
            Description = request.Description
        };

        var act = () => _controller.RegisterMotionSensor(request);
        _deviceServiceMock.Setup(x => x.RegisterMotionSensor(It.IsAny<RegisterMotionSensorArguments>(),
            It.IsAny<User>())).Returns(expectedMotionSensor);
        var response = _controller.RegisterMotionSensor(request);

        response.Should().NotBeNull();
        response.Id.Should().Be(expectedMotionSensor.Id);
        _deviceServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenRequestIsNull_ShouldThrowException()
    {
        var act = () => _controller.RegisterSmartLamp(null);
        
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenNameIsEmpty_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var act = () => _controller.RegisterSmartLamp(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenNameIsNull_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = null
        };

        var act = () => _controller.RegisterSmartLamp(request);
        
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenModelNumberIsNull_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "name",
            ModelNumber = null
        };

        var act = () => _controller.RegisterSmartLamp(request);
        
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'modelNumber')");
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenModelNumberIsEmpty_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "name",
            ModelNumber = ""
        };

        var act = () => _controller.RegisterSmartLamp(request);
        
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'modelNumber')");
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenMainPictureIsEmpty_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "",
            Photographies = new List<string>()
        };

        var act = () => _controller.RegisterSmartLamp(request);
        
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'mainPicture')");
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenMainPictureIsNull_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = null,
            Photographies = new List<string>()
        };

        var act = () => _controller.RegisterSmartLamp(request);
        
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value of parameter cannot be null or empty (Parameter 'mainPicture')");
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenRequestIsValid_ShouldRegisterDevice()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "SmartLamp",
            ModelNumber = "Model",
            Description = "Description",
            MainPicture = "MainPicture",
            Photographies = new List<string>()
        };

        var expectedSmartLamp = new Device
        {
            Name = request.Name,
            ModelNumber = request.ModelNumber,
            Description = request.Description,
            MainPicture = request.MainPicture,
            Photographies = request.Photographies
        };

        var act = () => _controller.RegisterSmartLamp(request);
        _deviceServiceMock.Setup(x => x.RegisterSmartLamp(It.IsAny<RegisterSmartLampArguments>(),
            It.IsAny<User>())).Returns(expectedSmartLamp);
        var response = _controller.RegisterSmartLamp(request);

        response.Should().NotBeNull();
        response.Id.Should().Be(expectedSmartLamp.Id);
        _deviceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetDevices_ShouldReturnDevices()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var deviceName = "deviceName";
        var modelNumber = "modelNumber";
        var companyName = "companyName";
        var mainPicture = "mainPicture";

        var devices = new List<Device>
        {
            new Device
            {
                Name = "name1",
                ModelNumber = "model1",
                MainPicture = "mainPicture1",
                Company = new Company
                {
                    Name = "company1"
                }
            },
            new Device
            {
                Name = "name2",
                ModelNumber = "model2",
                MainPicture = "mainPicture2",
                Company = new Company
                {
                    Name = "company2"
                }
            }
        };

        _deviceServiceMock.Setup(service => service.GetDevices(It.IsAny<GetAllDevicesArguments>())).Returns(devices);

        var result = _controller.ListDevices(pageNumber, pageSize, deviceName, modelNumber, companyName, mainPicture);
        
        result.Should().BeEquivalentTo(devices.Select(x => new GetAllDevicesDetailInfoResponse
        {
            Name = x.Name,
            ModelNumber = x.ModelNumber,
            MainPicture = x.MainPicture,
            CompanyName = x.Company.Name
        }));
        _deviceServiceMock.VerifyAll();
    }
    
    [TestMethod]
    public void GetSupportedTypes_ShouldReturnExpectedTypes()
    {
        var supportedTypes = new List<string> { "type1", "type2" };
        _deviceServiceMock.Setup(service => service.GetSupportedTypes()).Returns(supportedTypes);

        var result = _controller.ListSupportedTypes();
        
        result.Select(x => x.Type).Should().BeEquivalentTo(supportedTypes);
        _deviceServiceMock.Verify(service => service.GetSupportedTypes(), Times.Once);
    }

    [TestMethod]
    public void GetDeviceImportImplementations_ShouldReturnExpectedImplementations()
    {
        var implementations = new List<string> { "imp1", "imp2" };
        _deviceServiceMock.Setup(service => service.GetDeviceImportImplementations()).Returns(implementations);

        var result = _controller.GetDeviceImportImplementations();
        
        result.Should().BeEquivalentTo(implementations);
        _deviceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void ImportDevice_WhenRequestIsNull_ShouldThrowException()
    {
        var act = () => _controller.ImportDevice(null);
        
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }

    [TestMethod]
    public void ImportDevice_WhenRequestIsValid_ShouldImportDevices()
    {
        var request = new CreateDeviceImportRequest
        {
            Implementation = "imp1",
            FilePath = "path"
        };

        var devices = new List<Device>
        {
            new Device
            {
                Id = "id1"
            },
            new Device
            {
                Id = "id2"
            }
        };

        var expectedResponse = new CreateDeviceImportResponse(devices);

        _deviceServiceMock.Setup(service => service.Import(It.IsAny<CreateDeviceImportArguments>(), It.IsAny<User>()))
            .Returns(devices);

        var result = _controller.ImportDevice(request);
        
        result.Should().BeEquivalentTo(expectedResponse);
        _deviceServiceMock.VerifyAll();
    }
}