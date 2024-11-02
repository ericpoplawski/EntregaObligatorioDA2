namespace Domain.UserModels;

public sealed record class CreateHomeOwnerResponse
{
    public string Id { get; set; }
    
    public CreateHomeOwnerResponse(User userCreated)
    {
        Id = userCreated.Id;
    }
}