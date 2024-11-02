namespace Domain
{
    public class Session
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public User User { get; set;}
        
        public Session()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
    
    
}