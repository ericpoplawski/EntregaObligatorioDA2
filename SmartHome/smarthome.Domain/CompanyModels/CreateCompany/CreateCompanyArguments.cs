using CQ.Utility;

namespace Domain.CompanyModels;

public class CreateCompanyArguments
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public int RUT { get; set; }
    
    public CreateCompanyArguments(string name, string logo, int rut)
    {
        Guard.ThrowIsNullOrEmpty(name, nameof(name));
        Guard.ThrowIsNullOrEmpty(logo, nameof(logo));
        Name = name;
        Logo = logo;
        RUT = rut;
    }
    
    
}