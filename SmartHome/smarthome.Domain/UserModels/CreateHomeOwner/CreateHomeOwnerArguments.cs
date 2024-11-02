using CQ.Utility;

namespace Domain.UserModels;

public class CreateHomeOwnerArguments : CreateUserArguments
{
    public string ProfilePicture { get; set; }

    public CreateHomeOwnerArguments(string firstName, string lastName, string email, string password, string profilePicture)
        : base(firstName, lastName, email, password)
    {
        Guard.ThrowIsNullOrEmpty(profilePicture, nameof(profilePicture));
        ProfilePicture = profilePicture;
    }}