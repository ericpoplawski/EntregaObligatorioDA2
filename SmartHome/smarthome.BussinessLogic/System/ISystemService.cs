using Domain;
using Domain.UserModels;

namespace smarthome.Services.System.BusinessLogic
{
    public interface ISystemService
    {
        User AddAdministrator(CreateUserArguments arguments);
        
        User AddCompanyOwner(CreateUserArguments arguments);

        User AddHomeOwner(CreateHomeOwnerArguments arguments);

        List<User> GetUsers(GetAllUsersArguments arguments);

        User GetUserByEmail(string email);

        User UpdateIfUserIsComplete(User userReceived);

        void DeleteAdministratorById(string id);

        List<User> GetResidentsByHome(string homeId);

        User GetUserById(string userId);
        void AddResidentToUser(User user, Resident resident);
        //bool DoesUserHaveNotificationPermissionInSpecificHome(string userId, string homeId);
        void AddHomeOwnerRoleToUser(string userId);
    }
}
