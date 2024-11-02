using System.Linq.Expressions;
using Domain;
using Domain.CompanyModels;
using Domain.Exceptions;
using FluentAssertions;
using Moq;
using smarthome.BussinessLogic.Services.System;
using smarthome.Services.System.BusinessLogic;

namespace smarthome.UnitTests;

[TestClass]
public sealed class CompanyServiceTest
{
    private CompanyService _service;
    private Mock<IRepository<Company>> _mockCompanyRepository;
    private Mock<ISystemService> _systemServiceMock;
    
    [TestInitialize]
    public void Initialize()
    {
        _mockCompanyRepository = new Mock<IRepository<Company>>();
        _systemServiceMock = new Mock<ISystemService>();
        _service = new CompanyService(_mockCompanyRepository.Object, _systemServiceMock.Object);
    }
    
    [TestMethod]
    public void Create_WhenNameAlreadyExists_ShouldThrowException()
    {
        var request = new CreateCompanyRequest
        {
            Name = "name",
            Logo = "logo",
            RUT = 4303920,
        };
        
        var arguments = new CreateCompanyArguments(request.Name, request.Logo, request.RUT);
        var userLogged = new User();
        
        _mockCompanyRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Company, bool>>>())).Returns(true);
        
        var act = () => _service.CreateCompany(arguments, userLogged);
        
        act.Should().Throw<ServiceException>().WithMessage("There is already a company with this name");
        _mockCompanyRepository.VerifyAll();
    }
    
    [TestMethod]
    public void Create_WhenRUTAlreadyExists_ShouldThrowException()
    {
        var request = new CreateCompanyRequest
        {
            Name = "name",
            Logo = "logo",
            RUT = 3943085,
        };
    
        var userLogged = new User();
    
        var arguments = new CreateCompanyArguments(request.Name, request.Logo, request.RUT);
    
        _mockCompanyRepository.Setup(x => x.Exists(It.Is<Expression<Func<Company, bool>>>(expr => expr.Compile()(new Company { RUT = request.RUT }))))
            .Returns(true);
    
        var act = () => _service.CreateCompany(arguments, userLogged);
    
        act.Should().Throw<ServiceException>().WithMessage("There is already a company with this RUT");
        _mockCompanyRepository.VerifyAll();
    }
    
    [TestMethod]
    public void Create_WhenLogoAlreadyExists_ShouldThrowException()
    {
        var request = new CreateCompanyRequest
        {
            Name = "name",
            Logo = "logo",
            RUT = 3340292,
        };
        
        var userLogged = new User();
    
        var arguments = new CreateCompanyArguments(request.Name, request.Logo, request.RUT);
    
        _mockCompanyRepository.Setup(x => x.Exists(It.Is<Expression<Func<Company, bool>>>(expr => expr.Compile()(new Company { Logo = "logo" }))))
            .Returns(true);
    
        var act = () => _service.CreateCompany(arguments, userLogged);
    
        act.Should().Throw<ServiceException>().WithMessage("There is already a company with this logo");
        _mockCompanyRepository.VerifyAll();
    }
    
    [TestMethod]
    public void Create_WhenArgumentsAreValid_ShouldCreateCompany()
    {
        var request = new CreateCompanyRequest
        {
            Name = "name",
            Logo = "logo",
            RUT = 3110290,
        };
        
        var arguments = new CreateCompanyArguments(request.Name, request.Logo, request.RUT);
        
        var userLogged = new User();
        
        _mockCompanyRepository.Setup(x => x.Exists(It.IsAny<Expression<Func<Company, bool>>>())).Returns(false);
        _mockCompanyRepository.Setup(x => x.Add(It.IsAny<Company>()));
        _systemServiceMock.Setup(x => x.UpdateIfUserIsComplete(It.IsAny<User>()));
        
        var result = _service.CreateCompany(arguments, userLogged);
        
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Logo.Should().Be(request.Logo);
        result.RUT.Should().Be(request.RUT);
        _mockCompanyRepository.VerifyAll();
    }
    
    [TestMethod]
    public void Create_WhenIdIsSet_ShouldReturnCorrectId()
    {
        var expectedId = "1";
        
        var company = new Company { Id = expectedId };
        
        company.Id.Should().Be(expectedId);
        
    }
    
    [TestMethod]
    public void Create_WhenOwnerIsSet_ShouldReturnCorrectOwner()
    {
        var expectedOwner = "OwnerName";
        
        var company = new Company { Owner = new User { FullName = expectedOwner } };
        
        var result = company.Owner.FullName;

        result.Should().Be(expectedOwner);
    }

    [TestMethod]
    public void GetCompanies_ShouldReturnCompanies()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var companyName = "Tech Corp";
        var ownerName = "Alice Johnson";

        var companies = new List<Company>
        {
            new Company { Name = "Tech Corp", Owner = new User { FullName = "Alice Johnson" } },
            new Company { Name = "Biz Solutions", Owner = new User { FullName = "Bob Smith" } }
        };

        _mockCompanyRepository.Setup(x => x.GetAllWithPagination(
                pageNumber,
                pageSize,
                It.IsAny<Expression<Func<Company, bool>>>(),
                It.IsAny<Expression<Func<Company, object>>>()))
            .Returns(companies);

        var result = _service.GetCompanies(pageNumber, pageSize, companyName, ownerName);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().ContainSingle(c => c.Name == "Tech Corp");
        result.Should().ContainSingle(c => c.Name == "Biz Solutions");

        _mockCompanyRepository.VerifyAll();
    }

    [TestMethod]
    public void GetCompanyByUserId_ShouldReturnCorrectCompany()
    {
        var userId = "1";
        
        var company = new Company { Owner = new User { Id = userId } };
        
        _mockCompanyRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Company, bool>>>())).Returns(company);
        
        var result = _service.GetCompanyByUserId(userId);
        
        result.Should().NotBeNull();
        result.Owner.Id.Should().Be(userId);
    }
    
    [TestMethod]
    public void CreateCompany_WhenUserAlreadyHasACompany_ShouldThrowException()
    {
        var userLogged = new User { Id = "1" };
        var arguments = new CreateCompanyArguments("name", "logo", 3110290);

        _mockCompanyRepository.Setup(x => x.Exists(c => c.Owner.Id == userLogged.Id)).Returns(true);
        
        Action act = () => _service.CreateCompany(arguments, userLogged);
        
        act.Should().Throw<ServiceException>().WithMessage("User already has a company associated");
        _mockCompanyRepository.Verify(x => x.Exists(c => c.Owner.Id == userLogged.Id), Times.Once);
    }
    
    [TestMethod]
    public void GetCompanies_WhenCompanyNameAndOwnerNameAreNull_ShouldReturnAllCompanies()
    {
        var pageNumber = 1;
        var pageSize = 10;
        
        var companies = new List<Company>
        {
            new Company { Name = null, Owner = null },
            new Company { Name = null, Owner = null }
        };
        
        _mockCompanyRepository.Setup(x => x.GetAllWithPagination(
                pageNumber,
                pageSize,
                null,
                It.IsAny<Expression<Func<Company, object>>>()))
            .Returns(companies);
        
        var result = _service.GetCompanies(pageNumber, pageSize, null, null);
        
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        
        _mockCompanyRepository.VerifyAll();
    }
    
    [TestMethod]
    public void GetCompanies_WhenCompanyNameIsNotNullAndOwnerNameIsNull_ShouldReturnCompaniesWithCompanyName()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var companyName = "Tech Corp";
    
        var companies = new List<Company>
        {
            new Company { Name = "Tech Corp", Owner = null },
            new Company { Name = "Tech Corp", Owner = null }
        };
    
        _mockCompanyRepository.Setup(x => x.GetAllWithPagination(
                pageNumber,
                pageSize,
                It.Is<Expression<Func<Company, bool>>>(expr => expr.Compile()(new Company { Name = companyName })), 
                It.IsAny<Expression<Func<Company, object>>>()))
            .Returns(companies);
    
        var result = _service.GetCompanies(pageNumber, pageSize, companyName, null);
    
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    
        _mockCompanyRepository.VerifyAll();
    }
    
    [TestMethod]
    public void GetCompanies_WhenCompanyNameIsNullAndOwnerNameIsNotNull_ShouldReturnCompaniesWithOwnerName()
    {
        var pageNumber = 1;
        var pageSize = 10;
        var ownerName = "Alice Johnson";
    
        var companies = new List<Company>
        {
            new Company { Name = null, Owner = new User { FullName = "Alice Johnson" } },
            new Company { Name = null, Owner = new User { FullName = "Alice Johnson" } }
        };
    
        _mockCompanyRepository.Setup(x => x.GetAllWithPagination(
                pageNumber,
                pageSize,
                It.Is<Expression<Func<Company, bool>>>(expr => expr.Compile()(new Company { Owner = new User { FullName = ownerName } })), 
                It.IsAny<Expression<Func<Company, object>>>()))
            .Returns(companies);
    
        var result = _service.GetCompanies(pageNumber, pageSize, null, ownerName);
    
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    
        _mockCompanyRepository.VerifyAll();
    }
    
}