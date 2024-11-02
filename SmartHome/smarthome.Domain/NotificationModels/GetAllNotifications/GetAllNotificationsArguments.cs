namespace Domain.NotificationModels
{
    public class GetAllNotificationsArguments
    {
        public string DeviceType { get; set; }
        public DateTime CreationDatetime { get; set; }
        public string HasBeenRead { get; set; }

        public GetAllNotificationsArguments(string deviceType, DateTime creationDatetime, string hasBeenRead)
        {
            DeviceType = deviceType;
            CreationDatetime = creationDatetime;
            HasBeenRead = hasBeenRead;
        }
    }
}