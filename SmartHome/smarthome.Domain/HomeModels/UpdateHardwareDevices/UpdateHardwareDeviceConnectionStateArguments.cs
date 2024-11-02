using CQ.Utility;

namespace Domain.HomeModels
{
    public class UpdateHardwareDeviceConnectionStateArguments
    {
        public string NewConnectionState { get; set; }
        
        public UpdateHardwareDeviceConnectionStateArguments(string newConnectionState)
        {
            Guard.ThrowIsNullOrEmpty(newConnectionState, nameof(newConnectionState));
            NewConnectionState = newConnectionState;
        }
    }
}