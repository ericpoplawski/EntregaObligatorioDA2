namespace Domain.HomeModels
{
    public sealed record class ChangeHardwareDeviceConnectionStateRequest
    {
        public string NewConnectionState { get; set; }
    }
}