using Domain;
using Domain.NotificationModels;


namespace smarthome.BussinessLogic.Services.Notifications
{
    public interface INotificationService
    {
        CreateNotificationResponse CreateMotionDetectionNotification(string hardwareDeviceId);

        CreateNotificationResponse CreatePersonDetectionNotification(string hardwareDeviceId);

        CreateNotificationResponse CreateOpeningStateNotification(string hardwareDeviceId,
            ExecuteOpeningStateNotificationArguments arguments);

        CreateNotificationResponse CreatePowerStateNotification(string hardwareDeviceId,
            ExecutePowerStateNotificationArguments arguments);
        List<UserNotification> GetAllNotificationsByUser(string userId, GetAllNotificationsArguments arguments);

        UserNotification UpdateHasBeenRead(string userNotificationId);
    }
}