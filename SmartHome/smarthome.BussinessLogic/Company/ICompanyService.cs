using Domain;
using Domain.CompanyModels;

namespace smarthome.BussinessLogic.Services.System;

public interface ICompanyService
{
    Company CreateCompany(CreateCompanyArguments arguments, User userLogged);
    List<Company> GetCompanies(int pageNumber, int pageSize, string companyName, string ownerName);
    Company GetCompanyByUserId(string userId);
}