

namespace Domain.CompanyModels;

public sealed record class CreateCompanyRequest
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public int RUT { get; set; }

}