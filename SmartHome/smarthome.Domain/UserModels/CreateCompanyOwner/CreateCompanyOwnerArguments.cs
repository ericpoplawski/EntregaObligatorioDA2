namespace Domain.UserModels
{
    public class CreateCompanyOwnerArguments : CreateUserArguments
    {
        public CreateCompanyOwnerArguments(string firstName, string lastName, string email, string password)
            : base(firstName, lastName, email, password)
        {
        }
    }
}
