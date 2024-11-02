namespace Domain.HomeModels;

public sealed record class ChangeHardwareDeviceNameRequest
{
    public string NewName { get; set; }
}