using System.Linq.Expressions;
using Domain.DeviceModels;
using Domain.Exceptions;
using FluentAssertions;
using Moq;
using smarthome.BussinessLogic.Services.System;
using Domain;
using smarthome.BussinessLogic;
using Domain.DeviceModels.ImportDevice;
using smarthome.DataImporter;
using smarthome.DataImporter.ImporterFromJSON;
using smarthome.DataImporter.Entities;
using Domain.DeviceModels.RegisterSmartLamp;

namespace smarthome.UnitTests;

[TestClass]
public sealed class DeviceServiceTest
{
    private DeviceService _service;
    private Mock<IRepository<Device>> _mockDeviceRepository;
    private Mock<ICompanyService> _mockCompanyService;
    private Mock<ILoadAssembly<IDeviceImportService>> _loadAssemblyMock;

    [TestInitialize]
    public void Initialize()
    {
        _mockDeviceRepository = new Mock<IRepository<Device>>();
        _mockCompanyService = new Mock<ICompanyService>();
        _loadAssemblyMock = new Mock<ILoadAssembly<IDeviceImportService>>();
        _service = new DeviceService(_mockDeviceRepository.Object, _mockCompanyService.Object,
            _loadAssemblyMock.Object);
    }

    [TestMethod]
    public void RegisterSecurityCamera_WhenNameAlreadyExists_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>(),
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };

        var userLogged = new User();

        var arguments = new RegisterSecurityCameraArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies, request.MotionDetectionEnabled, request.PersonDetectionEnabled,
            request.UsageType);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(true);

        var act = () => _service.RegisterSecurityCamera(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this name");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void RegisterWindowSensor_WhenNameAlreadyExists_ShouldThrowException()
    {
        var request = new RegisterWindowSensorRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            Photographies = new List<string>(),
            MainPicture = "mainPicture"
        };

        var userLogged = new User();
        var arguments = new RegisterWindowSensorArguments(request.Name, request.ModelNumber, request.Description,
            request.Photographies, request.MainPicture);
        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(true);

        var act = () => _service.RegisterWindowSensor(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this name");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void RegisterSecurityCamera_WhenModelNumberAlreadyExists_ShouldThrowException()
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
            UsageType = "interior"
        };

        var userLogged = new User();

        var arguments = new RegisterSecurityCameraArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies, request.MotionDetectionEnabled, request.PersonDetectionEnabled,
            request.UsageType);

        _mockDeviceRepository.Setup(x =>
                x.Exists(It.Is<Expression<Func<Device, bool>>>(expr =>
                    expr.Compile()(new Device { ModelNumber = "modelNumber" }))))
            .Returns(true);

        var act = () => _service.RegisterSecurityCamera(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this model number");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void RegisterWindowSensor_WhenModelNumberAlreadyExists_ShouldThrowException()
    {
        var request = new RegisterWindowSensorRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            Photographies = new List<string>(),
            MainPicture = "mainPicture"
        };

        var userLogged = new User();

        var arguments = new RegisterWindowSensorArguments(request.Name, request.ModelNumber, request.Description,
            request.Photographies, request.MainPicture);

        _mockDeviceRepository.Setup(x =>
                x.Exists(It.Is<Expression<Func<Device, bool>>>(expr =>
                    expr.Compile()(new Device { ModelNumber = "modelNumber" }))))
            .Returns(true);

        var act = () => _service.RegisterWindowSensor(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this model number");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void RegisterSecurityCamera_WhenUsageTypeIsNotInteriorOrExterior_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string> { "photo1.jpg" },
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "invalid"
        };

        var userLogged = new User();

        var arguments = new RegisterSecurityCameraArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies, request.MotionDetectionEnabled, request.PersonDetectionEnabled,
            request.UsageType);

        var act = () => _service.RegisterSecurityCamera(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("Usage type must be interior or exterior");
    }

    [TestMethod]
    public void RegisterSecurityCamera_WhenUserHasNoCompanyAssociated_ShouldThrowException()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string> { "photo1.jpg" },
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };
        var arguments = new RegisterSecurityCameraArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies, request.MotionDetectionEnabled, request.PersonDetectionEnabled,
            request.UsageType);

        var userLogged = new User()
        {
        };

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns((Company)null);

        var act = () => _service.RegisterSecurityCamera(arguments, userLogged);

        act.Should().NotBeNull();
        act.Should().Throw<ServiceException>()
            .WithMessage("Users with no company associated, cannot register a device");
        _mockDeviceRepository.VerifyAll();
        _mockCompanyService.VerifyAll();
    }

    [TestMethod]
    public void RegisterSecurityCamera_WhenArgumentsAreValid_ShouldRegisterDevice()
    {
        var request = new RegisterSecurityCameraRequest()
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string> { "photo1.jpg" },
            MotionDetectionEnabled = true,
            PersonDetectionEnabled = true,
            UsageType = "interior"
        };
        var arguments = new RegisterSecurityCameraArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies, request.MotionDetectionEnabled, request.PersonDetectionEnabled,
            request.UsageType);

        var company = new Company()
        {
            Name = "Company"
        };

        var userLogged = new User()
        {
        };

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockDeviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns(company);


        var result = _service.RegisterSecurityCamera(arguments, userLogged);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.ModelNumber.Should().Be(request.ModelNumber);
        result.Type.Should().Be("securityCamera");

        _mockDeviceRepository.VerifyAll();
        _mockCompanyService.VerifyAll();
    }

    [TestMethod]
    public void RegisterWindowSensor_WhenUserHasNoCompanyAssociated_ShouldThrowException()
    {
        var request = new RegisterWindowSensorRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            Photographies = new List<string> { "photo1.jpg" },
            MainPicture = "mainPicture"
        };

        var userLogged = new User()
        {
        };

        var arguments = new RegisterWindowSensorArguments(request.Name, request.ModelNumber, request.Description,
            request.Photographies, request.MainPicture);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns((Company)null);

        var act = () => _service.RegisterWindowSensor(arguments, userLogged);

        act.Should().NotBeNull();
        act.Should().Throw<ServiceException>()
            .WithMessage("Users with no company associated, cannot register a device");


        _mockCompanyService.VerifyAll();
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void RegisterWindowSensor_WhenArgumentsAreValid_ShouldRegisterDevice()
    {
        var request = new RegisterWindowSensorRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            Photographies = new List<string> { "photo1.jpg" },
            MainPicture = "mainPicture"
        };

        var company = new Company()
        {
            Name = "Company"
        };

        var userLogged = new User()
        {
        };

        var arguments = new RegisterWindowSensorArguments(request.Name, request.ModelNumber, request.Description,
            request.Photographies, request.MainPicture);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockDeviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns(company);

        var result = _service.RegisterWindowSensor(arguments, userLogged);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.ModelNumber.Should().Be(request.ModelNumber);
        result.Type.Should().Be("windowSensor");

        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenNameAlreadyExists_ShouldThrowException()
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var userLogged = new User();

        var arguments = new RegisterMotionSensorArguments(request.Name, request.ModelNumber, request.Description,
                       request.Photographies, request.MainPicture);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(true);

        var act = () => _service.RegisterMotionSensor(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this name");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenModelNumberAlreadyExists_ShouldThrowException()
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var userLogged = new User();

        var arguments = new RegisterMotionSensorArguments(request.Name, request.ModelNumber, request.Description,
                       request.Photographies, request.MainPicture);

        _mockDeviceRepository.Setup(x =>
                       x.Exists(It.Is<Expression<Func<Device, bool>>>(expr =>
                                          expr.Compile()(new Device { ModelNumber = "modelNumber" }))))
            .Returns(true);

        var act = () => _service.RegisterMotionSensor(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this model number");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenUserHasNoCompanyAssociated_ShouldThrowException()
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var userLogged = new User();

        var arguments = new RegisterMotionSensorArguments(request.Name, request.ModelNumber, request.Description,
                                  request.Photographies, request.MainPicture);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns((Company)null);

        var act = () => _service.RegisterMotionSensor(arguments, userLogged);

        act.Should().NotBeNull();
        act.Should().Throw<ServiceException>()
            .WithMessage("Users with no company associated, cannot register a device");
        _mockDeviceRepository.VerifyAll();
        _mockCompanyService.VerifyAll();
    }

    [TestMethod]
    public void RegisterMotionSensor_WhenArgumentsAreValid_ShouldRegisterDevice()
    {
        var request = new RegisterMotionSensorRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var company = new Company()
        {
            Name = "Company"
        };

        var userLogged = new User();

        var arguments = new RegisterMotionSensorArguments(request.Name, request.ModelNumber, request.Description,
                                             request.Photographies, request.MainPicture);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockDeviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns(company);

        var result = _service.RegisterMotionSensor(arguments, userLogged);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.ModelNumber.Should().Be(request.ModelNumber);
        result.Type.Should().Be("motionSensor");

        _mockDeviceRepository.VerifyAll();
        _mockCompanyService.VerifyAll();
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenNameAlreadyExists_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var userLogged = new User();

        var arguments = new RegisterSmartLampArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(true);

        var act = () => _service.RegisterSmartLamp(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this name");
        _mockDeviceRepository.VerifyAll();
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenModelNumberAlreadyExists_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var userLogged = new User();

        var arguments = new RegisterSmartLampArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies);

        _mockDeviceRepository.Setup(x =>
            x.Exists(It.Is<Expression<Func<Device, bool>>>(expr =>
                expr.Compile()(new Device { ModelNumber = "modelNumber" })))
        ).Returns(true);

        var act = () => _service.RegisterSmartLamp(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this model number");
        _mockDeviceRepository.VerifyAll();
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenUserHasNoCompanyAssociated_ShouldThrowException()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var userLogged = new User();

        var arguments = new RegisterSmartLampArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns((Company)null);

        var act = () => _service.RegisterSmartLamp(arguments, userLogged);

        act.Should().NotBeNull();
        act.Should().Throw<ServiceException>()
            .WithMessage("Users with no company associated, cannot register a device");
        _mockDeviceRepository.VerifyAll();
        _mockCompanyService.VerifyAll();
    }
    
    [TestMethod]
    public void RegisterSmartLamp_WhenArgumentsAreValid_ShouldRegisterDevice()
    {
        var request = new RegisterSmartLampRequest
        {
            Name = "name",
            ModelNumber = "modelNumber",
            Description = "description",
            MainPicture = "mainPicture",
            Photographies = new List<string>()
        };

        var company = new Company()
        {
            Name = "Company"
        };

        var userLogged = new User();

        var arguments = new RegisterSmartLampArguments(request.Name, request.ModelNumber, request.Description,
            request.MainPicture, request.Photographies);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockDeviceRepository.Setup(x => x.Add(It.IsAny<Device>()));
        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns(company);

        var result = _service.RegisterSmartLamp(arguments, userLogged);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.ModelNumber.Should().Be(request.ModelNumber);
        result.Type.Should().Be("smartLamp");

        _mockDeviceRepository.VerifyAll();
        _mockCompanyService.VerifyAll();
    }

    [TestMethod]
    public void GetDevices_ShouldReturnDevices()
    {
        var devices = new List<Device>
        {
            new Device
            {
                Name = "device1", ModelNumber = "model1", MainPicture = "picture1",
                Company = new Company { Name = "company1" }
            },
            new Device
            {
                Name = "device2", ModelNumber = "model2", MainPicture = "picture2",
                Company = new Company { Name = "company2" }
            }
        };

        var arguments = new GetAllDevicesArguments(1, 10, "device1", "model1", "company1", "picture1");

        _mockDeviceRepository.Setup(x =>
                x.GetAllWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);

        var result = _service.GetDevices(arguments);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("device1");
        result[0].ModelNumber.Should().Be("model1");
        result[0].MainPicture.Should().Be("picture1");
        result[0].Company.Name.Should().Be("company1");
        result[1].Name.Should().Be("device2");
        result[1].ModelNumber.Should().Be("model2");
        result[1].MainPicture.Should().Be("picture2");
        result[1].Company.Name.Should().Be("company2");

        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void GetSupportedTypes_WhenCalled_ShouldReturnCorrectData()
    {
        var devices = new List<Device>
        {
            new Device { Type = "securityCamera" },
            new Device { Type = "windowSensor" },
            new Device { Type = "securityCamera" }
        };
        _mockDeviceRepository.Setup(repo => repo.GetAll(null)).Returns(devices);

        var result = _service.GetSupportedTypes();

        result.Should().BeEquivalentTo(new List<string> { "securityCamera", "windowSensor" });
    }


    [TestMethod]
    public void Should_Return_All_Devices_When_No_Arguments_Are_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, null, null, null);
        var devices = new List<Device> { new Device() };
        
        _mockDeviceRepository.Setup(r => r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize, It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }


    [TestMethod]
    public void Should_Return_Devices_By_DeviceType_When_Only_DeviceType_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, null, null, "Security Camera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_When_Only_DeviceName_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "Smart Bulb", null, null, null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Should_Return_Devices_By_ModelNumber_When_Only_ModelNumber_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, "SB1234", null, null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Should_Return_Devices_By_CompanyName_When_Only_CompanyName_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, null, "TechCorp", null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_And_DeviceType()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "Smart Bulb", null, null, "Security Camera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Should_Return_Devices_By_ModelNumber_And_DeviceType()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, "SB1234", null, "Security Camera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Should_Return_Devices_By_CompanyName_And_DeviceType()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, null, "TechCorp", "Security Camera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_And_CompanyName()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "Smart Bulb", "SB1234", "TechCorp", null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "Smart Bulb", "SB1234", "TechCorp", "Security Camera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_DeviceNameAndDeviceType_Are_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "Smart Bulb", null, null, "Security Camera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_ModelNumberAndDeviceType_Are_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, "SB1234", null, "Security Camera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_DeviceName_Is_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "device", null, null, null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_ModelNumber_Is_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, "model", null, null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_CompanyName_Is_Provided()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, null, "company", null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_DeviceNameAndDeviceTypeAreNull()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, "ahaha1", "company", null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_DeviceTypeAndModelNumber_Are_Null()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "device", null, "company", null);
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_DeviceNameIsNull()
    {
        var arguments = new GetAllDevicesArguments(1, 10, null, "ghjg8", "company", "securityCamera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_ModelNumberIsNull()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "device", null, "company", "securityCamera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }
    
    [TestMethod]
    public void Should_Return_Devices_By_DeviceName_ModelNumber_CompanyName_And_DeviceType_When_Only_CompanyNameIsNull()
    {
        var arguments = new GetAllDevicesArguments(1, 10, "device", "ghjg8", null, "securityCamera");
        var devices = new List<Device> { new Device() };
        _mockDeviceRepository.Setup(r =>
                r.GetAllWithPagination(arguments.PageNumber, arguments.PageSize,
                    It.IsAny<Expression<Func<Device, bool>>>()))
            .Returns(devices);
        
        var result = _service.GetDevices(arguments);
        
        result.Should().BeEquivalentTo(devices);
    }

    [TestMethod]
    public void GetDeviceById_WhenDeviceIsNotFound_ShouldThrowException()
    {
        var id = Guid.NewGuid().ToString();
        _mockDeviceRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Device, bool>>>())).Returns((Device)null);

        var act = () => _service.GetDeviceById(id);

        act.Should().Throw<EntityNotFoundException>().WithMessage("Device not found");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void GetDeviceById_WhenDeviceIsFound_ShouldReturnDevice()
    {
        var id = Guid.NewGuid().ToString();
        var device = new Device { Id = id };
        _mockDeviceRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Device, bool>>>())).Returns(device);

        var result = _service.GetDeviceById(id);

        result.Should().NotBeNull();
        result.Should().Be(device);
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void GetDeviceImportImplementations_ShouldReturnImplementations()
    {
        var implementations = new List<string> { "implementation1", "implementation2" };
        _loadAssemblyMock.Setup(x => x.GetImplementations(It.IsAny<string>())).Returns(implementations);

        var result = _service.GetDeviceImportImplementations();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(implementations);
        _loadAssemblyMock.VerifyAll();
    }

    [TestMethod]
    public void Import_WhenUserHasNoCompanyAssociated_ShouldThrowException()
    {
        var request = new CreateDeviceImportRequest
        {
            Implementation = "implementation",
            FilePath = "file"
        };

        var arguments = new CreateDeviceImportArguments(request.Implementation, request.FilePath);

        var userLogged = new User();

        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns((Company)null);

        var act = () => _service.Import(arguments, userLogged);

        act.Should().NotBeNull();
        act.Should().Throw<ServiceException>()
            .WithMessage("Users with no company associated, cannot import a device");
        _mockCompanyService.VerifyAll();
    }

    [TestMethod]
    public void Import_WhenNameAlreadyExists_ShouldThrowException()
    {
        var request = new CreateDeviceImportRequest
        {
            Implementation = "implementation",
            FilePath = "file"
        };

        var arguments = new CreateDeviceImportArguments(request.Implementation, request.FilePath);

        var company = new Company()
        {
            Name = "Company"
        };

        var userLogged = new User();

        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns(company);

        var deviceImportService = new Mock<IDeviceImportService>();

        deviceImportService
            .Setup(x => x.ImportDevice(It.IsAny<string>()))
            .Returns(new List<CreateDeviceFromImportArguments>
            {
                new CreateDeviceFromImportArguments
                {
                    Name = "Device1",
                    ModelNumber = "Model123",
                    Description = "Test device",
                    MainPicture = "picture.jpg",
                    Photographies = new List<string> { "pic1.jpg", "pic2.jpg" },
                    Type = "Sensor",
                    UsageType = "Indoor",
                    MotionDetectionEnabled = true,
                    PersonDetectionEnabled = false
                }
            });

        _loadAssemblyMock.As<ILoadAssembly<IDeviceImportService>>()
            .Setup(x => x.GetImplementation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(deviceImportService.Object);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(true);

        var act = () => _service.Import(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this name 'Device1'");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void Import_WhenModelNumberExists_ShouldThrowException()
    {
        var request = new CreateDeviceImportRequest
        {
            Implementation = "implementation",
            FilePath = "file"
        };

        var arguments = new CreateDeviceImportArguments(request.Implementation, request.FilePath);

        var company = new Company()
        {
            Name = "Company"
        };

        var userLogged = new User();

        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns(company);

        var deviceImportService = new Mock<IDeviceImportService>();

        deviceImportService
            .Setup(x => x.ImportDevice(It.IsAny<string>()))
            .Returns(new List<CreateDeviceFromImportArguments>
            {
                new CreateDeviceFromImportArguments
                {
                    Name = "Device1",
                    ModelNumber = "Model123",
                    Description = "Test device",
                    MainPicture = "picture.jpg",
                    Photographies = new List<string> { "pic1.jpg", "pic2.jpg" },
                    Type = "Sensor",
                    UsageType = "Indoor",
                    MotionDetectionEnabled = true,
                    PersonDetectionEnabled = false
                }
            });

        _loadAssemblyMock.As<ILoadAssembly<IDeviceImportService>>()
            .Setup(x => x.GetImplementation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(deviceImportService.Object);

        _mockDeviceRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Device, bool>>>())).Returns(false);
        _mockDeviceRepository.Setup(x =>
                       x.Exists(It.Is<Expression<Func<Device, bool>>>(expr => expr.Compile()(new Device { ModelNumber = "Model123" }))))
            .Returns(true);

        var act = () => _service.Import(arguments, userLogged);

        act.Should().Throw<ServiceException>().WithMessage("There is already a device with this model number 'Model123'");
        _mockDeviceRepository.VerifyAll();
    }

    [TestMethod]
    public void Import_WhenArgumentsAreValid_ShouldImportDevices()
    {
        var request = new CreateDeviceImportRequest
        {
            Implementation = "implementation",
            FilePath = "file"
        };

        var arguments = new CreateDeviceImportArguments(request.Implementation, request.FilePath);

        var company = new Company()
        {
            Name = "Company"
        };

        var userLogged = new User();

        _mockCompanyService.Setup(x => x.GetCompanyByUserId(It.IsAny<string>())).Returns(company);

        var deviceImportService = new Mock<IDeviceImportService>();

        deviceImportService
            .Setup(x => x.ImportDevice(It.IsAny<string>()))
            .Returns(new List<CreateDeviceFromImportArguments>
            {
            new CreateDeviceFromImportArguments
            {
                Name = "Device1",
                ModelNumber = "Model123",
                Description = "Test device",
                MainPicture = "picture.jpg",
                Photographies = new List<string> { "pic1.jpg", "pic2.jpg" },
                Type = "Sensor",
                UsageType = "Indoor",
                MotionDetectionEnabled = true,
                PersonDetectionEnabled = false
            }
            });

        _loadAssemblyMock.As<ILoadAssembly<IDeviceImportService>>()
            .Setup(x => x.GetImplementation(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(deviceImportService.Object);

        _mockDeviceRepository.Setup(x => x.Add(It.IsAny<Device>()));

        var result = _service.Import(arguments, userLogged);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Company.Name.Should().Be("Company");

        _mockDeviceRepository.VerifyAll();
        _mockCompanyService.VerifyAll();
        _loadAssemblyMock.VerifyAll();
    }


    [TestMethod]
    public void Device_Id_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var id = Guid.NewGuid().ToString();

        device.Id = id;

        device.Id.Should().Be(id);
    }

    [TestMethod]
    public void Device_Name_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var name = "Smart Camera";

        device.Name = name;

        device.Name.Should().Be(name);
    }

    [TestMethod]
    public void Device_ModelNumber_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var modelNumber = "SC1234";

        device.ModelNumber = modelNumber;

        device.ModelNumber.Should().Be(modelNumber);
    }

    [TestMethod]
    public void Device_Description_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var description = "This is a smart camera.";

        device.Description = description;

        device.Description.Should().Be(description);
    }

    [TestMethod]
    public void Device_MainPicture_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var mainPicture = "http://example.com/mainpicture.jpg";

        device.MainPicture = mainPicture;

        device.MainPicture.Should().Be(mainPicture);
    }

    [TestMethod]
    public void Device_Photographies_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var photographs = new List<string> { "http://example.com/photo1.jpg", "http://example.com/photo2.jpg" };

        device.Photographies = photographs;

        device.Photographies.Should().BeEquivalentTo(photographs);
    }

    [TestMethod]
    public void Device_Type_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var type = "Security Camera";

        device.Type = type;

        device.Type.Should().Be(type);
    }

    [TestMethod]
    public void Device_UsageType_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var usageType = "interior";

        device.UsageType = usageType;

        device.UsageType.Should().Be(usageType);
    }

    [TestMethod]
    public void Device_MotionDetectionEnabled_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var motionDetectionEnabled = true;

        device.MotionDetectionEnabled = motionDetectionEnabled;

        device.MotionDetectionEnabled.Should().Be(motionDetectionEnabled);
    }

    [TestMethod]
    public void Device_PersonDetectionEnabled_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var personDetectionEnabled = true;

        device.PersonDetectionEnabled = personDetectionEnabled;

        device.PersonDetectionEnabled.Should().Be(personDetectionEnabled);
    }

    [TestMethod]
    public void Device_CompanyId_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var companyId = Guid.NewGuid().ToString();

        device.CompanyId = companyId;

        device.CompanyId.Should().Be(companyId);
    }

    [TestMethod]
    public void Device_Company_Should_Get_And_Set_Value()
    {
        var device = new Device();
        var company = new Company { Name = "SmartHome Inc." };

        device.Company = company;

        device.Company.Should().Be(company);
    }

    [TestMethod]
    public void Device_Should_Initialize_Id()
    {
        var device = new Device();

        device.Id.Should().NotBeNullOrEmpty();
    }
}