namespace Domain.SessionModels
{
    public sealed record class SessionUserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> RoleNames { get; set; }

    }
}