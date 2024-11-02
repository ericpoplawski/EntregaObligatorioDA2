using CQ.Utility;

namespace Domain.DeviceModels;

public class RegisterSecurityCameraArguments
{
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string MainPicture { get; set; }
    public List<string> Photographies { get; set; }
    public bool MotionDetectionEnabled { get; set; }
    public bool PersonDetectionEnabled { get; set; }
    public string UsageType { get; set; }
    
    public RegisterSecurityCameraArguments(string name, string modelNumber, string description, string mainPicture, List<string> photographies, bool motionDetectedEnabled, bool personDetectedEnabled, string usageType)
    {
        Guard.ThrowIsNullOrEmpty(name, nameof(name));
        Guard.ThrowIsNullOrEmpty(modelNumber, nameof(modelNumber));
        Guard.ThrowIsNullOrEmpty(mainPicture, nameof(mainPicture));
        Guard.ThrowIsNullOrEmpty(usageType, nameof(usageType));
        Name = name;
        ModelNumber = modelNumber;
        Description = description;
        MainPicture = mainPicture;
        Photographies = photographies;
        MotionDetectionEnabled = motionDetectedEnabled;
        PersonDetectionEnabled = personDetectedEnabled;
        UsageType = usageType;
    }
}