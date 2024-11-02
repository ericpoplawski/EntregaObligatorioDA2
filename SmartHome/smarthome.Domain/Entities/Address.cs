namespace Domain;

public class Address
{
    public string Street { get; set; }
    public int HouseNumber { get; set; }
    
    
    public Address(string street, int houseNumber)
    {
        Street = street;
        HouseNumber = houseNumber;
    }
}