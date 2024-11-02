namespace Domain.HomeModels
{
    public sealed record class GetAllResidentsResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public List<HomePermission> HomePermissions { get; set; }
        public bool DoesUserMustBeNotified { get; set; }
    }
}