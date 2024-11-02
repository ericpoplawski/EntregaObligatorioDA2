namespace Domain.UserModels
{
    public sealed record class CreateAdministratorResponse
    {
        public string Id { get; set; }

        public CreateAdministratorResponse(User userCreated)
        {
            Id = userCreated.Id;
        }
    }
}
