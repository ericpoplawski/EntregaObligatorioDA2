namespace Domain;

public class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<Role> Roles { get; set; }
    public DateTime CreationDate { get; set; }
    public string? ProfilePicture { get; set; }
    public bool? IsComplete { get; set; }
    public List<Resident> Residents { get; set; }
    public bool HasPermission(PermissionKey permission)
    {
        return Roles.Any(role => role.SystemPermissions.Any(p => p.Name == permission.ToString()));
    }

    public User()
    {
        Id = Guid.NewGuid().ToString();
        Roles = new List<Role>();
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}