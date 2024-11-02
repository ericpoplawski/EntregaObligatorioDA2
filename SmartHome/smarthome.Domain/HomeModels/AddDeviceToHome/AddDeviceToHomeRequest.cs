namespace Domain.HomeModels;

public sealed record class AddDeviceToHomeRequest
{
    public string DeviceId { get; set; }

    public string RoomId { get; set; }
    

    public AddDeviceToHomeRequest(string deviceId)
    {
        DeviceId = deviceId;
    }
}