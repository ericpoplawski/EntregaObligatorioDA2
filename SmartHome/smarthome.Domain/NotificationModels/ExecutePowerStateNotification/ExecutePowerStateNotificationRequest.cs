namespace Domain.NotificationModels
{
    public sealed record class ExecutePowerStateNotificationRequest
    {
        public string NewPowerState { get; set; }
    }
}