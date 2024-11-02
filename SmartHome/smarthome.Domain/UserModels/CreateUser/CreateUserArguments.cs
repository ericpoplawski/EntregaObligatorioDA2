using CQ.Utility;

namespace Domain.UserModels
{
    public abstract class CreateUserArguments
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public CreateUserArguments(string firstName, string lastName, string email, string password)
        {
            ValidateUserArguments(firstName, lastName, email, password);
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }
        public void ValidateUserArguments(string firstName, string lastName, string email, string password)
        {
            Guard.ThrowIsNullOrEmpty(firstName, nameof(firstName));
            Guard.ThrowIsNullOrEmpty(lastName, nameof(lastName));
            Guard.ThrowIsNullOrEmpty(email, nameof(email));
            Guard.ThrowIsNullOrEmpty(password, nameof(password));
        }

    }
}
