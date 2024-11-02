namespace Domain;


public class Home
{
    public string Id { get; set; }
    public Address Address { get; set; }
    public Location Location { get; set; }
    public int QuantityOfResidents { get; set; }
    public int QuantityOfResidentsAllowed { get; set; }
    public string OwnerId { get; set; }
    public User Owner { get; set; }
    public string Alias { get; set; }

    public Home()
    {
        Id = Guid.NewGuid().ToString();
    }
    public Home(Address address, Location location, int quantityOfResidents, int quantityOfResidentsAllowed, User owner, User members, string alias)
    {
        Id = Guid.NewGuid().ToString();
        Address = address;
        Location = location;
        QuantityOfResidents = quantityOfResidents;
        QuantityOfResidentsAllowed = quantityOfResidentsAllowed;
        Owner = owner;
        OwnerId = owner.Id;
        Alias = alias;
    }
}