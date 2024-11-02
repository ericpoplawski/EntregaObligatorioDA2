namespace Domain.HomeModels
{
    public sealed record class GetResidentsDetailInfoResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public bool DoesUserMustBeNotified { get; set; }
        public List<string> HomePermissions { get; set; }

    }
}