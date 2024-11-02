namespace Domain.HomeModels;

public sealed record class AddRoomToHomeRequest
{
    public string Name { get; set; }
    
    public AddRoomToHomeRequest(string name)
    {
        Name = name;
    }
}