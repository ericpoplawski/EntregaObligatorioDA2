using Domain;
using Domain.Exceptions;
using Domain.HomeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smarthome.BussinessLogic.Services.HomeServices;
using smarthome.WebApi.Filters;

namespace smarthome.WebApi.Controllers;

[ApiController]
[AuthenticationFilter]
public class HomeController : SmartHomeControllerBase
{
    private readonly IHomeService _service;

    public HomeController(IHomeService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("api/homes")]
    [AuthorizationFilter("CreateHome")]
    public CreateHomeResponse CreateHome([FromBody] CreateHomeRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        User userLogged = GetUserLogged();
        CreateHomeArguments arguments = new CreateHomeArguments(request.Street, request.HouseNumber, request.Latitude,
            request.Longitude, request.QuantityOfResidents, request.QuantityOfResidentsAllowed, request.Alias);
        return new CreateHomeResponse(_service.CreateHome(arguments, userLogged));
    }

    [HttpPost]
    [Route("api/homes/{homeId}")]
    [AuthorizationFilter("AddResidentToHome")]
    public AddUserToHomeResponse AddUserToHome([FromRoute]string homeId, [FromBody]AddUserToHomeRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        string homeOwnerId = GetUserLogged().Id;
        AddUserToHomeArguments arguments = new AddUserToHomeArguments(request.UserId);
        return new AddUserToHomeResponse(_service.AddUserToHome(homeId, homeOwnerId, arguments));
    }
    
    [HttpPost]
    [Route("api/homes/{homeId}/devices")]
    [AuthorizationFilter("BindDeviceToHome")]
    public AddDeviceToHomeResponse AddDeviceToHome([FromRoute]string homeId, AddDeviceToHomeRequest request)
    {
         if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }

        string userLoggedId = GetUserLogged().Id;
        AddDeviceToHomeArguments arguments = new AddDeviceToHomeArguments(homeId, request.RoomId, request.DeviceId);
        return new AddDeviceToHomeResponse(_service.AddDeviceToHome(arguments, userLoggedId));
    }
    
    [HttpGet]
    [Route("api/homes/{id}/residents")]
    [AuthorizationFilter("ListHomeResidents")]
    public List<GetResidentsDetailInfoResponse> GetResidentsByHome([FromRoute] string homeId)
    {
        string userLoggedId = GetUserLogged().Id;
        var getResidentsResponse = _service.GetResidents(homeId, userLoggedId);

        List<GetResidentsDetailInfoResponse> responses =
            getResidentsResponse.Select(x => new GetResidentsDetailInfoResponse
            {
                FullName = x.FullName,
                Email = x.Email,
                ProfilePicture = x.ProfilePicture,
                HomePermissions = x.HomePermissions.Select(p => p.Name).ToList(),
                DoesUserMustBeNotified = x.DoesUserMustBeNotified
            }).ToList();
        return responses;
    }

    [HttpGet]
    [Route("api/homes/{id}/hardwareDevices")]
    [AuthorizationFilter("ListHomeDevices")]
    public List<GetHardwareDevicesDetailInfoResponse> ListHardwareDevicesByHome([FromRoute] string homeId, [FromQuery] string roomName = null)
    {
        string userLoggedId = GetUserLogged().Id;
        List<HardwareDevice> hardwareDevices = _service.GetHardwareDevicesByHome(homeId, userLoggedId, roomName);
        List<GetHardwareDevicesDetailInfoResponse> responses = hardwareDevices.Select(x => new GetHardwareDevicesDetailInfoResponse
        {
            DeviceName = x.Device.Name,
            DeviceModelNumber = x.Device.ModelNumber,
            MainPicture = x.Device.MainPicture,
            ConnectionState = x.ConnectionState,
            OpeningState = x.OpeningState,
            PowerState = x.PowerState
        }).ToList();
        return responses;
    }


    [HttpPut]
    [Route("api/homes/{id}/residentsPermissions")]
    public void ConfigureResidentsPermissions
        ([FromRoute]string homeId, ConfigureResidentsHomePermissionsRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        var arguments = new ConfigureResidentsHomePermissionsArguments(request.UserId, request.HomePermissionName);
        string userLoggedId = GetUserLogged().Id;
        _service.ConfigureResidentsPermissions(homeId, arguments, userLoggedId);
    }

    [HttpPut]
    [Route("api/hardwareDevices/{hardwareDeviceId}")]
    [AllowAnonymous]
    public void ChangeHardwareDeviceConnectionState([FromRoute] string hardwareDeviceId, ChangeHardwareDeviceConnectionStateRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }

        UpdateHardwareDeviceConnectionStateArguments arguments 
            = new UpdateHardwareDeviceConnectionStateArguments(request.NewConnectionState);
        _service.UpdateHardwareDeviceConnectionState(hardwareDeviceId, arguments);
    }
    
    [HttpPut]
    [Route("api/homes/{homeId}/hardwareDevices/{hardwareDeviceId}")]
    [AuthorizationFilter("ChangeHardwareDeviceName")]
    public void ChangeHardwareDeviceName([FromRoute] string homeId, [FromRoute] string hardwareDeviceId, ChangeHardwareDeviceNameRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        ChangeHardwareDeviceNameArguments arguments = new ChangeHardwareDeviceNameArguments(request.NewName);
        string userLoggedId = GetUserLogged().Id;
        _service.ChangeHardwareDeviceName(homeId, hardwareDeviceId, arguments, userLoggedId);
    }
    
    
    [HttpPut]
    [Route("api/homes/{homeId}/alias")]
    public void ChangeHomeAlias([FromRoute] string homeId, ChangeHomeAliasRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        
        ChangeHomeAliasArguments arguments = new ChangeHomeAliasArguments(request.Alias);
        
        _service.ChangeHomeAlias(homeId, arguments, GetUserLogged());
    }
    
    [HttpPost]
    [AuthorizationFilter("BindRoomToHome")]
    [Route("api/homes/{homeId}/rooms")]
    public void AddRoomToHome([FromRoute] string homeId, AddRoomToHomeRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        
        AddRoomToHomeArguments arguments = new AddRoomToHomeArguments(request.Name, homeId);
        
        _service.AddRoomToHome(arguments, GetUserLogged().Id);
        
    }
    
    
    
}