namespace Domain.NotificationModels
{
    public sealed record class ExecuteOpeningStateNotificationRequest
    {
        public string NewOpeningState { get; set; }
    }
}