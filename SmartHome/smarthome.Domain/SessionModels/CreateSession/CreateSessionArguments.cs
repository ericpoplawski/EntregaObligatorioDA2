using CQ.Utility;

namespace Domain.SessionModels
{
    public class CreateSessionArguments
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public CreateSessionArguments(string email, string password)
        {
            Guard.ThrowIsNullOrEmpty(email, nameof(email));
            Guard.ThrowIsNullOrEmpty(password, nameof(password));
            Email = email;
            Password = password;
        }
    }
}