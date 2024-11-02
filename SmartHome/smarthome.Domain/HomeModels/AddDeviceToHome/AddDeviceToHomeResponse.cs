namespace Domain.HomeModels;

public sealed record class AddDeviceToHomeResponse
{
    public string HardwareDeviceId { get; set; }

    public AddDeviceToHomeResponse(HardwareDevice hardwareDevice)
    {
        HardwareDeviceId = hardwareDevice.Id;
    }
}