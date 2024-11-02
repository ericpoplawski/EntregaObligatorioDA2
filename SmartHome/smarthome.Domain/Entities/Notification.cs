namespace Domain
{
    public class Notification
    {   
        public string Id { get; set; }
        public string Event { get; set; }
        public HardwareDevice HardwareDevice { get; set; }
        public DateTime CreationDatetime { get; set; }

        public Notification()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
