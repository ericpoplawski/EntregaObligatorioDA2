namespace Domain.DeviceModels
{
    public sealed record class RegisterMotionSensorResponse
    {
        public string Id { get; set; }
        public RegisterMotionSensorResponse(Device device) 
        {
            Id = device.Id;
        }
    }
}