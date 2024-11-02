namespace Domain;
public class Room
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Device> Devices { get; set; }
    public Home Home { get; set; }

    public Room()
    {
        Id = Guid.NewGuid().ToString();
    }
    
    public Room(string name, List<Device> devices, Home home)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Devices = devices;
        Home = home;
    }
}