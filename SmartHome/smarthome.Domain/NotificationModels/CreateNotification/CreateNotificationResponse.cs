namespace Domain.NotificationModels
{
    public sealed record class CreateNotificationResponse
    {
        public Notification Notification { get; set; }
        public List<UserNotification> UserNotifications { get; set; }
        public CreateNotificationResponse(Notification notification, List<UserNotification> userNotifications)
        {
            Notification = notification;
            UserNotifications = userNotifications;
        }

    }
}