using CQ.Utility;

namespace Domain.NotificationModels
{
    public class ExecutePowerStateNotificationArguments
    {
        public string NewPowerState { get; set; }

        public ExecutePowerStateNotificationArguments(string newPowerState)
        {
            Guard.ThrowIsNullOrEmpty(newPowerState, nameof(newPowerState));
            NewPowerState = newPowerState;
        }
    }
}