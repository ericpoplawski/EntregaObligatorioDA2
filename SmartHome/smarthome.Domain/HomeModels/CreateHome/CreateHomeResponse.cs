namespace Domain.HomeModels;

public sealed record class CreateHomeResponse
{
    public string Id { get; set; }

    public CreateHomeResponse(Home homeCreated)
    {
        Id = homeCreated.Id;
    }

}