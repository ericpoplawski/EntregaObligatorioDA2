namespace Domain;

public class Device
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string MainPicture { get; set; }
    public List<string> Photographies { get; set; }
    public string Type { get; set; }
    public string? UsageType { get; set; }
    public bool? MotionDetectionEnabled { get; set; }
    public bool? PersonDetectionEnabled { get; set; }
    public string CompanyId { get; set; }
    public Company Company { get; set; }

    public Device()
    {
        Id = Guid.NewGuid().ToString();
    }
    
}