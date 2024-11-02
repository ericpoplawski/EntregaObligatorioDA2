namespace Domain;


public class Company
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public int RUT { get; set; }
    public User Owner { get; set; }
    
    public Company()
    {
        Id = Guid.NewGuid().ToString();
    }
    
}