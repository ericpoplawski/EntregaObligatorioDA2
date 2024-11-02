using Domain.DeviceModels;
using Domain.DeviceModels.ImportDevice;
using Domain.DeviceModels.RegisterSmartLamp;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using smarthome.BussinessLogic.Services.System;
using smarthome.WebApi.Filters;

namespace smarthome.WebApi.Controllers;

[ApiController]
[AuthenticationFilter]
public sealed class DeviceController: SmartHomeControllerBase
{
    private readonly IDeviceService _service;
    public DeviceController(IDeviceService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Route("api/devices/securityCamera")]
    [AuthorizationFilter("RegisterSecurityCamera")]
    public RegisterSecurityCameraResponse RegisterSecurityCamera(RegisterSecurityCameraRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        
        var userLogged = GetUserLogged();

        var arguments = new RegisterSecurityCameraArguments
            (request.Name, request.ModelNumber, request.Description,request.MainPicture, request.Photographies, request.MotionDetectionEnabled, request.PersonDetectionEnabled, request.UsageType);
        var deviceCreated = _service.RegisterSecurityCamera(arguments, userLogged);
        
        return new RegisterSecurityCameraResponse(deviceCreated);
    }
    
    [HttpPost]
    [Route("api/devices/windowSensor")]
    [AuthorizationFilter("RegisterWindowSensor")]
    public RegisterWindowSensorResponse RegisterWindowSensor(RegisterWindowSensorRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        var userLogged = GetUserLogged();
        var arguments = new RegisterWindowSensorArguments
            (request.Name, request.ModelNumber, request.Description, request.Photographies, request.MainPicture);

        var deviceCreated = _service.RegisterWindowSensor(arguments, userLogged);
        
        var response = new RegisterWindowSensorResponse(deviceCreated);
        return response;
    }

    [HttpPost]
    [Route("api/devices/motionSensor")]
    [AuthorizationFilter("RegisterMotionSensor")]
    public RegisterMotionSensorResponse RegisterMotionSensor(RegisterMotionSensorRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        var userLogged = GetUserLogged();
        var arguments = new RegisterMotionSensorArguments
            (request.Name, request.ModelNumber, request.Description, request.Photographies, request.MainPicture);

        var deviceCreated = _service.RegisterMotionSensor(arguments, userLogged);

        return new RegisterMotionSensorResponse(deviceCreated);
    }

    [HttpPost]
    [Route("api/devices/smartLamp")]
    [AuthorizationFilter("RegisterSmartLamp")]
    public RegisterSmartLampResponse RegisterSmartLamp(RegisterSmartLampRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        } 
        var userLogged = GetUserLogged();
        var arguments = new RegisterSmartLampArguments
            (request.Name, request.ModelNumber, request.Description, request.MainPicture, request.Photographies);
        
        var deviceCreated = _service.RegisterSmartLamp(arguments, userLogged);
        
        return new RegisterSmartLampResponse(deviceCreated);
    }
    

    [HttpGet]
    [Route("api/devices")]
    [AuthorizationFilter("ListDevices")]
    public List<GetAllDevicesDetailInfoResponse> ListDevices([FromQuery] int pageNumber, [FromQuery] int pageSize,
        [FromQuery] string deviceName, [FromQuery] string modelNumber, [FromQuery] string companyName, [FromQuery] string deviceType)
    {
        GetAllDevicesArguments arguments = 
            new GetAllDevicesArguments(pageNumber, pageSize, deviceName, modelNumber, companyName, deviceType);

        var devices = _service.GetDevices(arguments);

        List<GetAllDevicesDetailInfoResponse> responses = 
            devices.Select(x => new GetAllDevicesDetailInfoResponse
            {
                Name = x.Name,
                ModelNumber = x.ModelNumber,
                MainPicture = x.MainPicture,
                CompanyName = x.Company.Name
            }).ToList();

        return responses;
    }

    [HttpGet]
    [Route("api/supportedDevices")]
    [AuthorizationFilter("ListSupportedDevices")]
    public List<GetSupportedTypesResponse> ListSupportedTypes()
    {
        List<string> supportedTypes = _service.GetSupportedTypes();
        List<GetSupportedTypesResponse> responses = new List<GetSupportedTypesResponse>();
        
        foreach (var type in supportedTypes)
        {
            responses.Add(new GetSupportedTypesResponse(type));
        }

        return responses;
    }

    [HttpGet]
    [Route("api/devices/deviceImportImplementations")]
    //[AuthorizationFilter("ListDeviceImportImplementations")]
    public List<string> GetDeviceImportImplementations()
    {
        List<string> implementations = _service.GetDeviceImportImplementations();
        return implementations;
    }

    [HttpPost]
    [Route("api/devices/deviceImport")]
    //[AuthorizationFilter("ImportDevice")]
    public CreateDeviceImportResponse ImportDevice(CreateDeviceImportRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        var userLogged = GetUserLogged();
        CreateDeviceImportArguments arguments = new
            CreateDeviceImportArguments(request.Implementation, request.FilePath);
        var devicesToSave = _service.Import(arguments, userLogged);
        return new CreateDeviceImportResponse(devicesToSave);
    }
}