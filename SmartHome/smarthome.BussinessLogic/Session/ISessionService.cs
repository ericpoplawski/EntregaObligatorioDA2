using Domain;
using Domain.SessionModels;

namespace smarthome.BussinessLogic.Services.Sessions
{
    public interface ISessionService
    {
        string CreateSession(CreateSessionArguments arguments);
        bool IsAuthorizationExpired(string authorizationHeader);
        User GetUserByToken(string authorization);
    }
}
