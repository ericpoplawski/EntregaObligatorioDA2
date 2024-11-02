namespace Domain.UserModels
{
    public class CreateAdministratorArguments : CreateUserArguments
    {
        public CreateAdministratorArguments(string firstName, string lastName, string email, string password)
            : base(firstName, lastName, email, password)
        {
        }
    }
}