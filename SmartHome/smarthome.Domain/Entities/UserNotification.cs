namespace Domain
{
    public class UserNotification
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string NotificationId { get; set; }
        public Notification Notification { get; set; }
        public bool HasBeenRead { get; set; }
        
        public UserNotification()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}