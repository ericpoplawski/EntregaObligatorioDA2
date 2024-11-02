using Domain;
using Domain.SessionModels;
using Microsoft.AspNetCore.Mvc;
using smarthome.BussinessLogic.Services.Sessions;

namespace smarthome.WebApi.Controllers
{
    [ApiController]
    public sealed class SessionController : SmartHomeControllerBase
    {
        private readonly ISessionService _service;

        public SessionController(ISessionService sessionService)
        {
            _service = sessionService;
        }

        [HttpPost]
        [Route("api/sessions")]
        public CreateSessionResponse CreateSession(CreateSessionRequest? request)
        {
            if (request == null)
            {
                throw new Exception("Request cannot be null");
            }

            var arguments = new CreateSessionArguments(request.Email, request.Password);
            var token = _service.CreateSession(arguments);
            var user = _service.GetUserByToken(token);

            List<string> roleNames = new List<string>();
            foreach (Role role in user.Roles)
            {
                roleNames.Add(role.RoleName);
            }
            
            var userResponse = new SessionUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                RoleNames = roleNames
            };
            
            var response = new CreateSessionResponse(token, userResponse);

            return response;
        }

    }
}
