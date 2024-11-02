namespace Domain.UserModels
{
    public sealed record class GetAllUsersDetailInfoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public List<string> RoleNames { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
