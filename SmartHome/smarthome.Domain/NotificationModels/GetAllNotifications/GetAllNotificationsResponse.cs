namespace Domain.NotificationModels
{
    public sealed record class GetAllNotificationsResponse
    {
        public string Event { get; set; }
        public string HardwareDeviceId { get; set; }
        public DateTime CreationDatetime { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModelNumber { get; set; }
        public bool HasBeenRead { get; set; }
    }
}