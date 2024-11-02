namespace Domain.DeviceModels.RegisterSmartLamp;

public sealed record class RegisterSmartLampResponse
{
    public string Id { get; set; }
    
    public RegisterSmartLampResponse(Device device)
    {
        Id = device.Id;
    }
}