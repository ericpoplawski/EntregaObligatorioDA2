namespace Domain.HomeModels;

public sealed record class AddUserToHomeResponse
{
    public Resident Membership { get; set; }
    
    public AddUserToHomeResponse(Resident membership)
    {
        Membership = membership;
    }
}