namespace Domain.DeviceModels
{
    public sealed record class GetAllDevicesDetailInfoResponse
    {
        public string Name { get; set; }
        public string ModelNumber { get; set; }
        public string MainPicture { get; set; }
        public string CompanyName { get; set; }
    }
}