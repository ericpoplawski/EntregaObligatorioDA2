namespace Domain.CompanyModels;

public sealed record class CreateCompanyResponse
{
    public string Id { get; set; }
    
    public CreateCompanyResponse(Company companyCreated)
    {
        Id = companyCreated.Id;
    }
    
}