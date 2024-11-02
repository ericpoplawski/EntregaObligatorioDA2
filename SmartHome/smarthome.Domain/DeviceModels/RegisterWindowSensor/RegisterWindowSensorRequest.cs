namespace Domain.DeviceModels;

public sealed record class RegisterWindowSensorRequest
{
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public List<string> Photographies { get; set; }
    public string MainPicture { get; set; }
}