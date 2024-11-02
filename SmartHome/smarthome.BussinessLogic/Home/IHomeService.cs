using Domain.HomeModels;
using Domain;

namespace smarthome.BussinessLogic.Services.HomeServices;

public interface IHomeService
{
    Home CreateHome (CreateHomeArguments arguments, User homeOwner);
    Resident AddUserToHome(string homeId, string homeOwnerId, AddUserToHomeArguments arguments);
    List<HomePermission> GetUserHomePermissions(string homeId, string userId);
    HardwareDevice AddDeviceToHome(AddDeviceToHomeArguments arguments, string userLoggedId);
    List<GetAllResidentsResponse> GetResidents(string homeId, string userLoggedId);
    List<HardwareDevice> GetHardwareDevicesByHome(string homeId, string userLoggedId, string roomName = null);
    List<HomePermission> ConfigureResidentsPermissions(string homeId, ConfigureResidentsHomePermissionsArguments arguments, string userLoggedId);
    HardwareDevice GetHardwareById(string hardwareDeviceId, bool includes = false);
    Home GetHomeById(string homeId);
    HardwareDevice UpdateHardwareDeviceConnectionState(string hardwareDeviceId, UpdateHardwareDeviceConnectionStateArguments arguments);
    Resident GetResidentById(string residentId);
    void ChangeHardwareDeviceName(string hardwareDeviceId, string homeId, ChangeHardwareDeviceNameArguments arguments, string userLoggedId);
    Home ChangeHomeAlias(string homeId, ChangeHomeAliasArguments arguments, User userLoggedId);
    Room AddRoomToHome(AddRoomToHomeArguments arguments, string userLoggedId);
    void UpdateHardwareDevice(HardwareDevice hardwareDevice);
    bool DoesUserHaveNotificationPermissionInSpecificHome(string userId, string homeId);
}