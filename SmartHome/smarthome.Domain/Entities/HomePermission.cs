namespace Domain;

public class HomePermission
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    public HomePermission()
    {
        Id = Guid.NewGuid().ToString();
    }
}