namespace Domain.UserModels;

public sealed record class CreateCompanyOwnerResponse
{
    public string Id { get; set; }
    
    public CreateCompanyOwnerResponse(User userCreated)
    {
        Id = userCreated.Id;
    }
}