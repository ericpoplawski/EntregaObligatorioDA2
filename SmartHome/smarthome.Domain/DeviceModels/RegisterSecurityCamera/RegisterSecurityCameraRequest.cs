namespace Domain.DeviceModels;
public sealed record class RegisterSecurityCameraRequest
{
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string MainPicture { get; set; }
    public List<string> Photographies { get; set; }
    public bool MotionDetectionEnabled { get; set; }
    public bool PersonDetectionEnabled { get; set; }
    public string UsageType { get; set; }
}