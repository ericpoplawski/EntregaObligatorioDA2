namespace Domain.DeviceModels;

public sealed record class RegisterWindowSensorResponse
{
    public string Id { get; set; }
    
    public RegisterWindowSensorResponse(Device device)
    {
        Id = device.Id;
    }
}