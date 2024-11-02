using Domain;
using Domain.Exceptions;
using Domain.NotificationModels;
using smarthome.BussinessLogic.Services.HomeServices;
using smarthome.Services.System.BusinessLogic;

namespace smarthome.BussinessLogic.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private IHomeService _homeService;
        private ISystemService _systemService;
        private IRepository<Notification> _notificationRepository;
        private IRepository<UserNotification> _userNotificationRepository;

        public NotificationService(IHomeService homeService, ISystemService systemService,
            IRepository<Notification> notificationRepository, IRepository<UserNotification> userNotificationRepository) 
        {
            _homeService = homeService;
            _systemService = systemService;
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
        }
        public CreateNotificationResponse CreateMotionDetectionNotification(string hardwareDeviceId)
        {
            var hardwareDevice = _homeService.GetHardwareById(hardwareDeviceId, true);
            var action = "motionDetectionNotification";
            ValidateHardwareDeviceInNotificationCreation(hardwareDevice, action);
            
            if (hardwareDevice.Device.Type == "securityCamera")
            {
                if (!hardwareDevice.Device.MotionDetectionEnabled.HasValue || !hardwareDevice.Device.MotionDetectionEnabled.Value)
                {
                    throw new ServiceException("Motion detection is not enabled on this device");
                }
            }

            var notification = new Notification()
            {
                Event = "motionDetection",
                HardwareDevice = hardwareDevice,
                CreationDatetime = DateTime.Now
            };

            List<User> users = _systemService.GetResidentsByHome(hardwareDevice.Home.Id);
            Home home = _homeService.GetHomeById(hardwareDevice.Home.Id);
            User owner = _systemService.GetUserById(home.OwnerId);

            List<UserNotification> userNotifications = new List<UserNotification>();
            foreach (User user in users)
            {
                var hasPermission = _homeService.DoesUserHaveNotificationPermissionInSpecificHome(user.Id, home.Id);
                if (hasPermission)
                {
                    UserNotification userNotification = new UserNotification()
                    {
                        User = user,
                        Notification = notification,
                        HasBeenRead = false
                    };
                    userNotifications.Add(userNotification);
                }
            }

            userNotifications.Add(new UserNotification()
            {
                User = owner,
                Notification = notification,
                HasBeenRead = false
            });

            _notificationRepository.Add(notification);
            foreach (var userNotification in userNotifications)
            {
                _userNotificationRepository.Add(userNotification);
            }

            return new CreateNotificationResponse(notification, userNotifications);

        }

        public CreateNotificationResponse CreatePersonDetectionNotification(string hardwareDeviceId)
        {
            var hardwareDevice = _homeService.GetHardwareById(hardwareDeviceId, true);

            var action = "personDetectionNotification";
            ValidateHardwareDeviceInNotificationCreation(hardwareDevice, action);

            if (!hardwareDevice.Device.PersonDetectionEnabled.HasValue || !hardwareDevice.Device.PersonDetectionEnabled.Value)            {
                throw new ServiceException("Person detection is not enabled on this device");
            }

            var notification = new Notification()
            {
                Event = "personDetection",
                HardwareDevice = hardwareDevice,
                CreationDatetime = DateTime.Now
            };

            List<User> users = _systemService.GetResidentsByHome(hardwareDevice.Home.Id);
            Home home = _homeService.GetHomeById(hardwareDevice.Home.Id);
            User owner = home.Owner;

            List<UserNotification> userNotifications = new List<UserNotification>();
            foreach (User user in users)
            {
                var hasPermission = _homeService.DoesUserHaveNotificationPermissionInSpecificHome(user.Id, home.Id);
                if (hasPermission)
                {
                    UserNotification userNotification = new UserNotification()
                    {
                        User = user,
                        Notification = notification,
                        HasBeenRead = false
                    };
                    userNotifications.Add(userNotification);
                }
            }

            userNotifications.Add(new UserNotification()
            {
                User = owner,
                Notification = notification,
                HasBeenRead = false
            });

            _notificationRepository.Add(notification);
            foreach (var userNotification in userNotifications)
            {
                _userNotificationRepository.Add(userNotification);
            }

            return new CreateNotificationResponse(notification, userNotifications);
        }

        public CreateNotificationResponse CreateOpeningStateNotification(string hardwareDeviceId, 
            ExecuteOpeningStateNotificationArguments arguments)
        {
            var hardwareDevice = _homeService.GetHardwareById(hardwareDeviceId, true);
            var action = "openingStateNotification";

            ValidateHardwareDeviceInNotificationCreation(hardwareDevice, action);
            if (arguments.NewOpeningState != "open" && arguments.NewOpeningState != "closed")
            {
                throw new ServiceException("Opening state must be 'open' or 'closed'");
            }
            if (arguments.NewOpeningState == hardwareDevice.OpeningState)
            {
                throw new ServiceException("Device is already in the specified state");
            }

            hardwareDevice.OpeningState = arguments.NewOpeningState;
            _homeService.UpdateHardwareDevice(hardwareDevice);

            var notification = new Notification()
            {
                Event = "openingStateChanged",
                HardwareDevice = hardwareDevice,
                CreationDatetime = DateTime.Now
            };

            List<User> users = _systemService.GetResidentsByHome(hardwareDevice.Home.Id);
            Home home = _homeService.GetHomeById(hardwareDevice.Home.Id);
            User owner = home.Owner;

            List<UserNotification> userNotifications = new List<UserNotification>();
            foreach (User user in users)
            {
                var hasPermission = _homeService.DoesUserHaveNotificationPermissionInSpecificHome(user.Id, home.Id);
                if (hasPermission)
                {
                    UserNotification userNotification = new UserNotification()
                    {
                        User = user,
                        Notification = notification,
                        HasBeenRead = false
                    };
                    userNotifications.Add(userNotification);
                }
            }

            userNotifications.Add(new UserNotification()
            {
                User = owner,
                Notification = notification,
                HasBeenRead = false
            });

            _notificationRepository.Add(notification);
            foreach (var userNotification in userNotifications)
            {
                _userNotificationRepository.Add(userNotification);
            }

            return new CreateNotificationResponse(notification, userNotifications);
        }

        public CreateNotificationResponse CreatePowerStateNotification(string hardwareDeviceId,
            ExecutePowerStateNotificationArguments arguments)
        {
            HardwareDevice hardwareDevice = _homeService.GetHardwareById(hardwareDeviceId, true);

            string action = "powerStateNotification";
            ValidateHardwareDeviceInNotificationCreation(hardwareDevice, action);

            if (arguments.NewPowerState != "ON" && arguments.NewPowerState != "OFF")
            {
                throw new ServiceException("Power state must be 'ON' or 'OFF'");
            }
            if (arguments.NewPowerState == hardwareDevice.PowerState)
            {
                throw new ServiceException("Device is already in the specified state");
            }

            hardwareDevice.PowerState = arguments.NewPowerState;
            _homeService.UpdateHardwareDevice(hardwareDevice);

            var notification = new Notification()
            {
                Event = "powerStateChanged",
                HardwareDevice = hardwareDevice,
                CreationDatetime = DateTime.Now
            };

            List<User> users = _systemService.GetResidentsByHome(hardwareDevice.Home.Id);
            Home home = _homeService.GetHomeById(hardwareDevice.Home.Id);
            User owner = home.Owner;

            List<UserNotification> userNotifications = new List<UserNotification>();
            foreach (User user in users)
            {
                var hasPermission = _homeService.DoesUserHaveNotificationPermissionInSpecificHome(user.Id, home.Id);
                if (hasPermission)
                {
                    UserNotification userNotification = new UserNotification()
                    {
                        User = user,
                        Notification = notification,
                        HasBeenRead = false
                    };
                    userNotifications.Add(userNotification);
                }
            }

            userNotifications.Add(new UserNotification()
            {
                User = owner,
                Notification = notification,
                HasBeenRead = false
            });

            _notificationRepository.Add(notification);
            foreach (var userNotification in userNotifications)
            {
                _userNotificationRepository.Add(userNotification);
            }

            return new CreateNotificationResponse(notification, userNotifications);
        }

        public void ValidateHardwareDeviceInNotificationCreation(HardwareDevice hardwareDevice, string action)
        {
            List<string> supportedTypesForPowerStateNotification = new List<string> { "smartLamp" };
            List<string> supportedTypesForOpeningStateNotification = new List<string> { "windowSensor" };
            List<string> supportedTypesForMotionDetectionNotification = 
                new List<string> { "securityCamera", "motionSensor" };
            List<string> supportedTypesForPersonDetectionNotification = new List<string> { "securityCamera" };

            if (hardwareDevice == null)
            {
                throw new EntityNotFoundException("Hardware device not found");
            }

            if (action == "powerStateNotification" && !supportedTypesForPowerStateNotification.Contains(hardwareDevice.Device.Type))
            {
                throw new ServiceException($"Hardware device of type '{hardwareDevice.Device.Type}' does not support this action");
            }
            else if (action == "openingStateNotification" && !supportedTypesForOpeningStateNotification.Contains(hardwareDevice.Device.Type))
            {
                throw new ServiceException($"Hardware device of type '{hardwareDevice.Device.Type}' does not support this action");
            }
            else if (action == "motionDetectionNotification" && !supportedTypesForMotionDetectionNotification.Contains(hardwareDevice.Device.Type))
            {
                throw new ServiceException($"Hardware device of type '{hardwareDevice.Device.Type}' does not support this action");
            }
            else if (action == "personDetectionNotification" && !supportedTypesForPersonDetectionNotification.Contains(hardwareDevice.Device.Type))
            {
                throw new ServiceException($"Hardware device of type '{hardwareDevice.Device.Type}' does not support this action");
            }

            if (hardwareDevice.ConnectionState != "connected")
            {
                throw new ServiceException("Hardware device is not connected");
            }
        }

        public List<UserNotification> GetAllNotificationsByUser(string userId, GetAllNotificationsArguments arguments)
        {
            if (arguments.HasBeenRead != "Yes" && arguments.HasBeenRead.ToLower() != "No" 
                                               && !string.IsNullOrEmpty(arguments.HasBeenRead))
            {
                throw new ServiceException("Argument has been read must be 'Yes', 'No' or 'Empty'");
            }

            bool IsRead = false;
            if (arguments.HasBeenRead == "Yes")
            {
                IsRead = true;
            }

            DateTime creationDatetime = arguments.CreationDatetime == DateTime.MinValue ? DateTime.Now : arguments.CreationDatetime;

            if (!string.IsNullOrEmpty(arguments.DeviceType) && !string.IsNullOrEmpty(arguments.HasBeenRead))
            {
                return _userNotificationRepository.GetAll
                (x => x.User.Id == userId && x.Notification.CreationDatetime == creationDatetime
                                          && x.HasBeenRead == IsRead && x.Notification.HardwareDevice.Device.Type == arguments.DeviceType);
            }
            if (!string.IsNullOrEmpty(arguments.DeviceType))
            {
                return _userNotificationRepository.GetAll
                (x => x.User.Id == userId && x.Notification.CreationDatetime == creationDatetime
                                          && x.Notification.HardwareDevice.Device.Type == arguments.DeviceType);
            }
            if (!string.IsNullOrEmpty(arguments.HasBeenRead))
            {
                return _userNotificationRepository.GetAll
                (x => x.User.Id == userId && x.Notification.CreationDatetime == creationDatetime
                                          && x.HasBeenRead == IsRead);
            }
            return _userNotificationRepository.GetAll(x => x.User.Id == userId);
        }

        public UserNotification UpdateHasBeenRead(string userNotificationId)
        {
            var userNotification = _userNotificationRepository.Get(x => x.Id == userNotificationId);
            if (userNotification == null)
            {
                throw new EntityNotFoundException("User notification not found");
            }

            userNotification.HasBeenRead = true;
            _userNotificationRepository.Update(userNotification);
            return userNotification;
        }
        
    }
}