namespace Domain
{
    public sealed record class SystemPermission
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Role> Roles { get; set; }
        public SystemPermission()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}