using Domain;
using smarthome.Services.System.BusinessLogic;
using Domain.SessionModels;
using Domain.Exceptions;

namespace smarthome.BussinessLogic.Services.Sessions
{
    public class SessionService : ISessionService
    {
        private ISystemService _systemService;
        private readonly IRepository<Session> _repository;

        public SessionService(ISystemService systemService, IRepository<Session> sessionRepository)
        {
            _systemService = systemService;
            _repository = sessionRepository;
        }

        public string CreateSession(CreateSessionArguments arguments)
        {
            User user = _systemService.GetUserByEmail(arguments.Email);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            else
            {
                CheckIfPasswordIsCorrect(user.Password, arguments.Password);
                var token = Guid.NewGuid().ToString();
                _repository.Add(new Session
                {
                    Token = token,
                    User = user
                });
                return token;
            }
        }

        public bool IsAuthorizationExpired(string authorizationHeader)
        {
            var session = _repository.Get(s => s.Token == authorizationHeader);
            return session == null;
        }
        public User GetUserByToken(string authorizationHeader)
        {
            var session = _repository.Get(
                s => s.Token == authorizationHeader,
                s => s.User);

            User user = _systemService.GetUserById(session.User.Id);

            if (session == null)
                throw new EntityNotFoundException("Session not found");
            return user;
        }

        public void CheckIfPasswordIsCorrect(string passwordUser, string passwordArguments)
        {
            if (passwordUser != passwordArguments)
                throw new ServiceException("Invalid password");
        }
    }
}
