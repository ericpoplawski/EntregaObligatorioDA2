using CQ.Utility;

namespace Domain.HomeModels;

public class AddRoomToHomeArguments
{
    public string Name { get; set; }
    public string HomeId { get; set; }
    
    public AddRoomToHomeArguments(string name, string homeId)
    {
        Guard.ThrowIsNullOrEmpty(name, nameof(name));
        Name = name;
        HomeId = homeId;
    }
   
}