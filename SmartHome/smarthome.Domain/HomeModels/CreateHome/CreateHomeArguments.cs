using CQ.Utility;
namespace Domain.HomeModels;

public class CreateHomeArguments
{
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int QuantityOfResidents { get; set; }
    public int QuantityOfResidentsAllowed { get; set; }
    public string Alias { get; set; }
    public CreateHomeArguments(string street, int houseNumber, double latitude, double longitude, int quantityOfResidents, int quantityOfResidentsAllowed, string alias)
    {
        ValidateHomeArguments(street, alias);
        Street = street;
        HouseNumber = houseNumber;
        Latitude = latitude;
        Longitude = longitude;
        QuantityOfResidents = quantityOfResidents;
        QuantityOfResidentsAllowed = quantityOfResidentsAllowed;
        Alias = alias;
    }
    
    private void ValidateHomeArguments(string street,string alias)
    {
       Guard.ThrowIsNullOrEmpty(street, nameof(street)); 
       Guard.ThrowIsNullOrEmpty(alias, nameof(alias));
    }
}