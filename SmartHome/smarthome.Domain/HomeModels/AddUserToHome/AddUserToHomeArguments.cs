using CQ.Utility;

namespace Domain.HomeModels;

public class AddUserToHomeArguments
{
    public string UserId { get; set; }

    public AddUserToHomeArguments(string userId)
    {
        Guard.ThrowIsNullOrEmpty(userId, nameof(userId));
        UserId = userId;
    }
}