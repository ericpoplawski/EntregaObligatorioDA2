using Domain;
using Domain.CompanyModels;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using smarthome.BussinessLogic.Services.System;
using smarthome.WebApi.Filters;


namespace smarthome.WebApi.Controllers;

[ApiController]
[AuthenticationFilter]
public sealed class CompanyController : SmartHomeControllerBase
{
    private readonly ICompanyService _service;
    
    public CompanyController(ICompanyService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Route("api/companies")]
    [AuthorizationFilter("CreateCompany")]
    public CreateCompanyResponse CreateCompany(CreateCompanyRequest request)
    {
        if (request == null)
        {
            throw new ControllerException("Request cannot be null");
        }
        var userLogged = GetUserLogged();
        CreateCompanyArguments arguments = new CreateCompanyArguments(request.Name, request.Logo, request.RUT);
        Company companyCreated = _service.CreateCompany(arguments, userLogged);
        CreateCompanyResponse response = new CreateCompanyResponse(companyCreated);
        return response;
    }
    
    [HttpGet]
    [Route("api/companies")]
    [AuthorizationFilter("ListCompanies")]
    public List<GetAllCompaniesDetailInfoResponse> ListCompanies([FromQuery] int pageNumber, [FromQuery] int pageSize,
        [FromQuery] string companyName = null, [FromQuery] string ownerName = null)
    {
        GetAllCompaniesArguments arguments = 
            new GetAllCompaniesArguments(pageNumber, pageSize, companyName, ownerName);

        var companies = _service.GetCompanies(arguments.PageNumber,
            arguments.PageSize, arguments.CompanyName, arguments.OwnerName);

        List<GetAllCompaniesDetailInfoResponse> responses =
            companies.Select(c => new GetAllCompaniesDetailInfoResponse
            {
                CompanyName = c.Name,
                OwnerName = c.Owner.FullName,
                OwnerEmail = c.Owner.Email,
                CompanyRUT = c.RUT
            }).ToList();
        return responses;
    }
}