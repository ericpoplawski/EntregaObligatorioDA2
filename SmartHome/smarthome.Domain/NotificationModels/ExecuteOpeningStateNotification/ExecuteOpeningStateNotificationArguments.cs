using CQ.Utility;

namespace Domain.NotificationModels
{
    public class ExecuteOpeningStateNotificationArguments
    {
        public string NewOpeningState { get; set; }

        public ExecuteOpeningStateNotificationArguments(string newOpeningState)
        {
            Guard.ThrowIsNullOrEmpty(newOpeningState, nameof(newOpeningState));
            NewOpeningState = newOpeningState;
        }
    }
}