using Domain;
using Domain.DeviceModels;
using Domain.DeviceModels.ImportDevice;
using Domain.DeviceModels.RegisterSmartLamp;

namespace smarthome.BussinessLogic.Services.System;

public interface IDeviceService
{
    Device RegisterSecurityCamera(RegisterSecurityCameraArguments arguments, User userLogged);
    Device RegisterWindowSensor(RegisterWindowSensorArguments arguments, User userLogged);
    Device RegisterMotionSensor(RegisterMotionSensorArguments arguments, User userLogged);
    Device RegisterSmartLamp(RegisterSmartLampArguments arguments, User userLogged);
    List<Device> GetDevices(GetAllDevicesArguments arguments);
    List<string> GetSupportedTypes();
    Device GetDeviceById(string id);
    List<string> GetDeviceImportImplementations();
    List<Device> Import(CreateDeviceImportArguments arguments, User userLogged);
}