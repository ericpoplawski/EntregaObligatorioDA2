using CQ.Utility;
using Domain;
using Domain.CompanyModels;
using Domain.Exceptions;
using smarthome.Services.System.BusinessLogic;


namespace smarthome.BussinessLogic.Services.System;

public class CompanyService : ICompanyService
{
    private readonly IRepository<Company> _companyRepository;
    private ISystemService _systemService;
    
    public CompanyService(IRepository<Company> companyRepository, ISystemService systemService)
    {
        _companyRepository = companyRepository;
        _systemService = systemService;
    }

    public Company CreateCompany(CreateCompanyArguments arguments, User userLogged)
    {
        ValidateName(arguments.Name);
        ValidateLogo(arguments.Logo);
        ValidateRUT(arguments.RUT);
        if (_companyRepository.Exists(c => c.Owner.Id == userLogged.Id))
        {
            throw new ServiceException("User already has a company associated");
        }
        Company company = new Company()
        {
            Name = arguments.Name,
            Logo = arguments.Logo,
            RUT = arguments.RUT,
            Owner = userLogged
        };
        _companyRepository.Add(company);
        _systemService.UpdateIfUserIsComplete(userLogged);
        return company;
    }
    

    private void ValidateName(string name)
    {
        if(_companyRepository.Exists(x => x.Name == name))
        {
            throw new ServiceException("There is already a company with this name");
        }
    }
    
    private void ValidateLogo(string logo)
    {
        Guard.ThrowIsNullOrEmpty(logo, "logo");
        if(_companyRepository.Exists(x => x.Logo == logo))
        {
            throw new ServiceException("There is already a company with this logo");
        }
    }
    
    private void ValidateRUT(int rut)
    {
        if(_companyRepository.Exists(x => x.RUT == rut))
        {
            throw new ServiceException("There is already a company with this RUT");
        }
    }
    
    public List<Company> GetCompanies(int pageNumber, int pageSize, string companyName, string ownerName)
    {
        if (string.IsNullOrEmpty(companyName) && string.IsNullOrEmpty(ownerName))
        {
            return _companyRepository.GetAllWithPagination(pageNumber, pageSize, null, c => c.Owner);
        }
        if (!string.IsNullOrEmpty(companyName) && string.IsNullOrEmpty(ownerName))
        {
            return _companyRepository.GetAllWithPagination(pageNumber, pageSize, x => x.Name == companyName, c => c.Owner);
        }
        if (string.IsNullOrEmpty(companyName) && !string.IsNullOrEmpty(ownerName))
        {
            return _companyRepository.GetAllWithPagination(pageNumber, pageSize, x => x.Owner.FullName == ownerName, c => c.Owner);
        }
        return _companyRepository.GetAllWithPagination(pageNumber, pageSize,
                x => x.Name == companyName && x.Owner.FullName == ownerName, c => c.Owner);
    }

    public Company GetCompanyByUserId(string userId)
    {
        return _companyRepository.Get(x => x.Owner.Id == userId);
    }
}