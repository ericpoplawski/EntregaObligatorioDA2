namespace Domain.DeviceModels;

public sealed record RegisterSecurityCameraResponse
{
    public string Id { get; set; }
    
    public RegisterSecurityCameraResponse(Device device)
    {
        Id = device.Id;
    }
}