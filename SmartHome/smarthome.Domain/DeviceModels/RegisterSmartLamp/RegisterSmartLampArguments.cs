using CQ.Utility;

namespace Domain.DeviceModels.RegisterSmartLamp;

public class RegisterSmartLampArguments
{
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public string Description { get; set; }
    public string MainPicture { get; set; }
    public List<string> Photographies { get; set; }
    
    public RegisterSmartLampArguments(string name, string modelNumber, string description, string mainPicture, List<string> photographies)
    {
        Guard.ThrowIsNullOrEmpty(name, nameof(name));
        Guard.ThrowIsNullOrEmpty(modelNumber, nameof(modelNumber));
        Guard.ThrowIsNullOrEmpty(mainPicture, nameof(mainPicture));
        Name = name;
        ModelNumber = modelNumber;
        Description = description;
        MainPicture = mainPicture;
        Photographies = photographies;
    }
}