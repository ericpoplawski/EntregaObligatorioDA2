namespace Domain
{
    public sealed record class Role
    {
        public string Id { get; set; }

        public string RoleName { get; set; }
        public List<SystemPermission> SystemPermissions { get; set; }
        
        public Role()
        {
            Id = Guid.NewGuid().ToString();
            SystemPermissions = new List<SystemPermission>();
        }
    }
}