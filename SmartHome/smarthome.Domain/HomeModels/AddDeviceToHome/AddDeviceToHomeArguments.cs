namespace Domain.HomeModels;

public class AddDeviceToHomeArguments
{
    public string HomeId { get; set;  }
    public string DeviceId { get; set; }
    public string RoomId { get; set; }
    
    public AddDeviceToHomeArguments(string homeId, string roomId, string deviceId)
    {
        HomeId = homeId;
        RoomId = roomId;
        DeviceId = deviceId;
    }
}