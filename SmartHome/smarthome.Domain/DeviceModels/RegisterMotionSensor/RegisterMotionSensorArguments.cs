using CQ.Utility;

namespace Domain.DeviceModels
{
    public class RegisterMotionSensorArguments
    {
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public string Description { get; set; }
        public List<string> Photographies { get; set; }
        public string MainPicture { get; set; }

        public RegisterMotionSensorArguments(string name, string modelNumber,
            string description, List<string> photographies, string mainPicture)
        {
            Guard.ThrowIsNullOrEmpty(name, nameof(name));
            Guard.ThrowIsNullOrEmpty(modelNumber, nameof(modelNumber));
            Name = name;
            ModelNumber = modelNumber;
            Description = description;
            Photographies = photographies;
            MainPicture = mainPicture;
        }
    }
}