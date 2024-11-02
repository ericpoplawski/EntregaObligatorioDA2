namespace Domain.HomeModels;

public sealed record class CreateHomeRequest
{
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int QuantityOfResidents { get; set; }
    public int QuantityOfResidentsAllowed { get; set; }
    public string Alias { get; set; }
}