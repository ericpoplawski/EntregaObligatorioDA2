namespace Domain.HomeModels
{
    public sealed record class GetHardwareDevicesDetailInfoResponse
    {
        public string DeviceName { get; set; }
        public string DeviceModelNumber { get; set; }

        public string MainPicture { get; set; }
        public string ConnectionState { get; set; }
        public string? OpeningState { get; set; }
        public string? PowerState { get; set; }
    }
}