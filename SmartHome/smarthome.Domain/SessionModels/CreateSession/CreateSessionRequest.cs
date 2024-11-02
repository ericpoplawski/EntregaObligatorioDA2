namespace Domain.SessionModels
{
    public sealed record class CreateSessionRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}