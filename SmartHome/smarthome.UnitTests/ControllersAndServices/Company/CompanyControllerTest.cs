using Azure.Core;
using Domain;
using Domain.CompanyModels;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using smarthome.BussinessLogic.Services.System;
using smarthome.WebApi.Controllers;

namespace smarthome.UnitTests;

[TestClass]
public class CompanyControllerTest
{
    private CompanyController _controller;
    private Mock<ICompanyService> _companyServiceMock;
    private Mock<HttpContext> _httpContextMock;


    [TestInitialize]
    public void Initialize()
    {
        _companyServiceMock = new Mock<ICompanyService>(MockBehavior.Strict);
        _httpContextMock = new Mock<HttpContext>(MockBehavior.Strict);
        ControllerContext controllerContext = new ControllerContext
        {
            HttpContext = _httpContextMock.Object
        };
        _controller = new CompanyController(_companyServiceMock.Object);
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
                             Name = PermissionKey.CreateCompany.ToString()
                        }
                    }
                }
            }
        };
        _httpContextMock.SetupGet(c => c.Items[Items.UserLogged]).Returns(user);
    }
    
    [TestMethod]
    public void CreateCompany_WhenRequestIsNull_ShouldThrowException()
    {
        Action act = () => _controller.CreateCompany(null);
        
        act.Should().Throw<ControllerException>().WithMessage("Request cannot be null");
    }
    
    [TestMethod]
    public void Create_WhenNameIsEmpty_ShouldThrowException()
    {
        var request = new CreateCompanyRequest
        {
            Name = "",
            Logo = "logo",
            RUT = 2940203,
        };

        var act = () => _controller.CreateCompany(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }
    
    [TestMethod]
    public void Create_WhenNameIsNull_ShouldThrowException()
    {
        var request = new CreateCompanyRequest
        {
            Name = null,
            Logo = "logo",
            RUT = 4302930,
        };

        var act = () => _controller.CreateCompany(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'name')");
    }
    
    [TestMethod]
    public void Create_WhenLogoIsEmpty_ShouldThrowException()
    {
        var request = new CreateCompanyRequest
        {
            Name = "name",
            Logo = "",
            RUT = 4111020,
        };
        
        var act = () => _controller.CreateCompany(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'logo')");
    }
    
    [TestMethod]
    public void Create_WhenLogoIsNull_ShouldThrowException()
    {
        var request = new CreateCompanyRequest
        {
            Name = "name",
            Logo = null,
            RUT = 3999021,
        };

        var act = () => _controller.CreateCompany(request);
        
        act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'logo')");
    }

    [TestMethod]
    public void CreateCompany_WhenRequestIsValid_ShouldReturnCompany()
    {
        var request = new CreateCompanyRequest
        {
            Name = "Company",
            Logo = "Logo",
            RUT = 3010192
        };

        var company = new Company()
        {
            Name = request.Name,
            Logo = request.Logo,
            RUT = request.RUT
        };

        _companyServiceMock.Setup(x => x.CreateCompany
        (It.IsAny<CreateCompanyArguments>(), It.IsAny<User>())).Returns(company);

        var response = _controller.CreateCompany(request);

        response.Should().NotBeNull();
        response.Id.Should().Be(company.Id);
        _companyServiceMock.VerifyAll();
        _httpContextMock.VerifyAll();
    }

    [TestMethod]
    public void GetCompanies_ShouldReturnCompanies()
    {
        int pageNumber = 1;
        int pageSize = 10;

        var request = new GetAllCompaniesDetailInfoResponse
        {
            CompanyName = "Company",
            OwnerName = "Owner",
            OwnerEmail = "",
            CompanyRUT = 23413323
        };

        var companies = new List<Company>
        {
            new Company
            {
                Name = request.CompanyName,
                Owner = new User
                {
                    FullName = request.OwnerName,
                    Email = request.OwnerEmail
                },
                RUT = request.CompanyRUT
            }
        };

        _companyServiceMock.Setup(x => x.GetCompanies(pageNumber, pageSize, null, null)).Returns(companies);

        var response = _controller.ListCompanies(pageNumber, pageSize, null, null);

        response.Should().NotBeNull();
        response.Should().HaveCount(1);
        response[0].CompanyName.Should().Be(request.CompanyName);
        response[0].OwnerName.Should().Be(request.OwnerName);
        response[0].OwnerEmail.Should().Be(request.OwnerEmail);
        response[0].CompanyRUT.Should().Be(request.CompanyRUT);
        _companyServiceMock.VerifyAll();
    }
}