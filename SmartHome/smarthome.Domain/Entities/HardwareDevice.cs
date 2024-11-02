namespace Domain;

public class HardwareDevice
{
    public string Id { get; set; }
    public string DeviceId { get; set; }
    public Device Device { get; set; }
    public string HomeId { get; set; }
    public Home Home { get; set; }
    public string ConnectionState { get; set; }
    public string? OpeningState { get; set; }
    public string? PowerState { get; set; }
    public string Name { get; set; }
    public Room Room { get; set; }

    public HardwareDevice(Device device)
    {
        Device = device;
        Id = Guid.NewGuid().ToString();
        ConnectionState = "connected";
        Name = Device.Name;
    }

    public HardwareDevice()
    {
        Id = Guid.NewGuid().ToString();
        ConnectionState = "connected";
    }
}