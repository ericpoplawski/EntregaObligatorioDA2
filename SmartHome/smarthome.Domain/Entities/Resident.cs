namespace Domain;

public class Resident
{
    public string Id { get; set; }
    public string HomeId { get; set; }
    public Home Home { get; set; }
    public List<HomePermission> HomePermissions { get; set; }
    
    public Resident()
    {
        Id = Guid.NewGuid().ToString();
        HomePermissions = new List<HomePermission>();
    }
    public Resident(Home home, List<HomePermission> homePermissions)
    {
        Id = Guid.NewGuid().ToString();
        Home = home;
        HomeId = home.Id;
        HomePermissions = homePermissions;
    }
}