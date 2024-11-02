using Domain.UserModels;
using Microsoft.AspNetCore.Mvc;
using smarthome.Services.System.BusinessLogic;
using smarthome.WebApi.Controllers;
using smarthome.WebApi.Filters;
using Domain;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[AuthenticationFilter]
public sealed class SystemController : SmartHomeControllerBase
{
    private readonly ISystemService _service;
    public SystemController(ISystemService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("api/administrators")]
    [AuthorizationFilter("CreateAdministrator")]
    public CreateAdministratorResponse CreateAdministrator(CreateAdministratorRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }

        CreateAdministratorArguments arguments = new CreateAdministratorArguments
            (request.FirstName, request.LastName, request.Email, request.Password);

        User userCreated = _service.AddAdministrator(arguments);
        return new CreateAdministratorResponse(userCreated);
    }
    
    [HttpPost]
    [Route("api/companyOwners")]
    [AuthorizationFilter("CreateCompanyOwner")]
    public CreateCompanyOwnerResponse CreateCompanyOwner(CreateCompanyOwnerRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        CreateCompanyOwnerArguments arguments = new CreateCompanyOwnerArguments
            (request.FirstName, request.LastName, request.Email, request.Password);
        
        User userCreated = _service.AddCompanyOwner(arguments);
        
        return new CreateCompanyOwnerResponse(userCreated);
    }
    
    [HttpPost]
    [Route("api/homeOwners")]
    [AllowAnonymous]
    public CreateHomeOwnerResponse CreateHomeOwner(CreateHomeOwnerRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        
        CreateHomeOwnerArguments arguments = new CreateHomeOwnerArguments
            (request.FirstName, request.LastName, request.Email, request.Password, request.ProfilePicture);
        
        User userCreated = _service.AddHomeOwner(arguments);
        
        return new CreateHomeOwnerResponse(userCreated);
        
    }

    
    [HttpGet]
    [Route("api/users")]
    [AuthorizationFilter("ListUsers")]
    public List<GetAllUsersDetailInfoResponse> GetUsers([FromQuery] int pageNumber, [FromQuery] int pageSize,
    [FromQuery] string roleName = null, [FromQuery] string fullName = null)
    {
        GetAllUsersArguments arguments = new GetAllUsersArguments(fullName, roleName, pageNumber, pageSize);

        var users = _service.GetUsers(arguments);

        List<GetAllUsersDetailInfoResponse> responses = users
            .Select(x => new GetAllUsersDetailInfoResponse
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                FullName = x.FullName,
                RoleNames = x.Roles.Select(r => r.RoleName).ToList(),
                CreationDate = x.CreationDate
            })
            .ToList();

        return responses;
    }

    [HttpDelete]
    [Route("api/administrators/{id}")]
    [AuthorizationFilter("DeleteAdministrator")]
    public void DeleteAdministratorById([FromRoute]string id)
    {
        var isValidId = Guid.TryParse(id, out _);
        if (!isValidId)
            throw new ControllerException("Id is not valid");
        _service.DeleteAdministratorById(id);
    }

    [HttpPut]
    [Route("api/users/{id}/addHomeOwnerRole")]
    [AuthorizationFilter("AddHomeOwnerRoleToUser")]
    public void AddHomeOwnerRoleToUser([FromRoute]string id)
    {
        _service.AddHomeOwnerRoleToUser(id);
    }
}