namespace Domain.HomeModels;

public sealed record class AddUserToHomeRequest
{
    public string UserId { get; set; }
    
}