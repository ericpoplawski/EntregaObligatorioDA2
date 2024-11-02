namespace Domain.DeviceModels.RegisterSmartLamp;

public sealed record class RegisterSmartLampRequest
{
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string MainPicture { get; set; }
    public List<string> Photographies { get; set; }
}