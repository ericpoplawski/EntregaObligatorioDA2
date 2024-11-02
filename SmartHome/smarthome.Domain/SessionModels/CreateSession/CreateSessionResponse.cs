namespace Domain.SessionModels
{
    public sealed record class CreateSessionResponse
    {
        public string Token { get; set; }
        public SessionUserResponse User { get; set; }
        public CreateSessionResponse(string token, SessionUserResponse user)
        {
            Token = token;
            User = user;
        }
    }
}