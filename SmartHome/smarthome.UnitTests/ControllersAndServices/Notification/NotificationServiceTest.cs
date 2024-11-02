using Domain;
using Domain.Exceptions;
using Domain.NotificationModels;
using FluentAssertions;
using Moq;
using smarthome.BussinessLogic.Services.HomeServices;
using smarthome.BussinessLogic.Services.Notifications;
using smarthome.Services.System.BusinessLogic;
using System.Linq.Expressions;

namespace smarthome.UnitTests.NotificationTests
{
    [TestClass]
    public sealed class NotificationServiceTest
    {
        private NotificationService _service;
        private Mock<IHomeService> _homeService;
        private Mock<ISystemService>_systemService;
        private Mock<IRepository<Notification>>_notificationRepositoryMock;
        private Mock<IRepository<UserNotification>>_userNotificationRepositoryMock;
        private Device _exampleSecurityCamera;
        private Device _exampleWindowSensor;


        [TestInitialize]
        public void Initialize()
        {
            _homeService = new Mock<IHomeService>();
            _systemService = new Mock<ISystemService>();
            _notificationRepositoryMock = new Mock<IRepository<Notification>>();
            _userNotificationRepositoryMock = new Mock<IRepository<UserNotification>>();
            
            _service = new NotificationService(
            _homeService.Object,
            _systemService.Object,
            _notificationRepositoryMock.Object,
            _userNotificationRepositoryMock.Object);
            
            _exampleSecurityCamera = new Device
            {
                Id = "deviceId",
                Name = "Camera",
                ModelNumber = "Model123",
                Type = "securityCamera"
            };
            _exampleWindowSensor = new Device
            {
                Id = "deviceId",
                Name = "Window Sensor",
                ModelNumber = "Model123",
                Type = "windowSensor"
            };
        }

        [TestMethod]
        public void CreateMotionDetectionNotification_WhenHardwareDeviceIsNull_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";
            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns((HardwareDevice)null);

            var act = () => _service.CreateMotionDetectionNotification(hardwareDeviceId);

            act.Should().Throw<EntityNotFoundException>().WithMessage("Hardware device not found");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreateMotionDetectionNotification_WhenHardwareDeviceDoesNotSupportAction_ShouldReturnResponse()
        {
            var hardwareDeviceId = "someDevice";
            var hardwareDevice = new HardwareDevice(_exampleWindowSensor)
            {
                Id = hardwareDeviceId
            };

            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns(hardwareDevice);

            var act = () => _service.CreateMotionDetectionNotification(hardwareDeviceId);

            act.Should().Throw<ServiceException>().WithMessage($"Hardware device of type '{hardwareDevice.Device.Type}' does not support this action");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreateMotionDetectionNotification_WhenHardwareDeviceIsNotConnected_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                ConnectionState = "disconnected",
            };

            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns(hardwareDevice);

            var act = () => _service.CreateMotionDetectionNotification(hardwareDeviceId);

            act.Should().Throw<ServiceException>().WithMessage("Hardware device is not connected");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreateMotionDetectionNotification_WhenMotionDetectionIsNotEnabled_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            _exampleSecurityCamera.MotionDetectionEnabled = false;
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                ConnectionState = "connected",
            };

            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns(hardwareDevice);
            var act = () => _service.CreateMotionDetectionNotification(hardwareDeviceId);
            act.Should().Throw<ServiceException>().WithMessage("Motion detection is not enabled on this device");
        }

        [TestMethod]
        public void CreateMotionDetectionNotification_WhenChecksArePassed_ShouldReturnResponse()
        {
            var hardwareDeviceId = "someDevice";
            var homeId = "someHomeId";
            var owner = new User { Id = "ownerId" };
            _exampleSecurityCamera.MotionDetectionEnabled = true;
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                ConnectionState = "connected",
                Home = new Home() { Id = homeId, Owner = owner }
            };
            var users = new List<User>
            {
                new User { Id = "user1" },
                new User { Id = "user2" }
            };

            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns(hardwareDevice);
            _homeService.Setup(x => x.GetHomeById(homeId)).Returns(hardwareDevice.Home);
            _systemService.Setup(x => x.GetResidentsByHome(homeId)).Returns(users);
            _homeService.Setup(x => x.DoesUserHaveNotificationPermissionInSpecificHome(It.IsAny<string>(), homeId)).Returns(true);
            _notificationRepositoryMock.Setup(x => x.Add(It.IsAny<Notification>()));
            _userNotificationRepositoryMock.Setup(x => x.Add(It.IsAny<UserNotification>()));

            var response = _service.CreateMotionDetectionNotification(hardwareDeviceId);

            response.Should().NotBeNull();
            response.Notification.Should().NotBeNull();
            response.UserNotifications.Should().HaveCount(users.Count + 1);

            _homeService.VerifyAll();
            _systemService.VerifyAll();
            _notificationRepositoryMock.VerifyAll();
            _userNotificationRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void CreatePersonDetectionNotification_WhenHardwareDeviceIsNull_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";
            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns((HardwareDevice)null);

            var act = () => _service.CreatePersonDetectionNotification(hardwareDeviceId);

            act.Should().Throw<EntityNotFoundException>().WithMessage("Hardware device not found");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePersonDetectionNotification_WhenHardwareDoesNotSupportAction_ShouldReturnResponse()
        {
            var hardwareDeviceId = "someDevice";
            var hardwareDevice = new HardwareDevice(_exampleWindowSensor)
            {
                Id = hardwareDeviceId,
            };

            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns(hardwareDevice);

            var act = () => _service.CreatePersonDetectionNotification(hardwareDeviceId);

            act.Should().Throw<ServiceException>().WithMessage($"Hardware device of type '{hardwareDevice.Device.Type}' does not support this action");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePersonDetectionNotification_WhenHardwareDeviceIsNotConnected_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                ConnectionState = "disconnected",
            };

            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns(hardwareDevice);

            var act = () => _service.CreatePersonDetectionNotification(hardwareDeviceId);

            act.Should().Throw<ServiceException>().WithMessage("Hardware device is not connected");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePersonDetectionNotification_WhenPersonDetectionIsNotEnabled_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            _exampleSecurityCamera.PersonDetectionEnabled = false;
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                ConnectionState = "connected",
            };

            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns(hardwareDevice);
            var act = () => _service.CreatePersonDetectionNotification(hardwareDeviceId);
            act.Should().Throw<ServiceException>().WithMessage("Person detection is not enabled on this device");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePersonDetectionNotification_WhenChecksArePassed_ShouldReturnResponse()
        {
            var hardwareDeviceId = "someDevice";
            var homeId = "someHomeId";
            var owner = new User { Id = "ownerId" };
            _exampleSecurityCamera.PersonDetectionEnabled = true;
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                ConnectionState = "connected",
                Home = new Home() { Id = homeId, Owner = owner }
            };
            var users = new List<User>
            {
                new User { Id = "user1" },
                new User { Id = "user2" }
            };

            _homeService.Setup(x => x.GetHardwareById(hardwareDeviceId, true)).Returns(hardwareDevice);
            _homeService.Setup(x => x.GetHomeById(homeId)).Returns(hardwareDevice.Home);
            _systemService.Setup(x => x.GetResidentsByHome(homeId)).Returns(users);
            _homeService.Setup(x => x.DoesUserHaveNotificationPermissionInSpecificHome(It.IsAny<string>(), homeId)).Returns(true);
            _notificationRepositoryMock.Setup(x => x.Add(It.IsAny<Notification>()));
            _userNotificationRepositoryMock.Setup(x => x.Add(It.IsAny<UserNotification>()));

            var response = _service.CreatePersonDetectionNotification(hardwareDeviceId);

            response.Should().NotBeNull();
            response.Notification.Should().NotBeNull();
            response.UserNotifications.Should().HaveCount(users.Count + 1);

            _homeService.VerifyAll();
            _systemService.VerifyAll();
            _notificationRepositoryMock.VerifyAll();
            _userNotificationRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void CreateOpeningStateNotification_WhenHardwareDeviceIsNull_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";

            var arguments = new ExecuteOpeningStateNotificationArguments("someState");
            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns((HardwareDevice)null);

            var act = () => _service.CreateOpeningStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<EntityNotFoundException>().WithMessage("Hardware device not found");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreateOpeningStateNotification_WhenHardwareDeviceDoesNotSupportAction_ShouldReturnResponse()
        {
            var hardwareDeviceId = "someDevice";
            var arguments = new ExecuteOpeningStateNotificationArguments("someState");
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
            };

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            var act = () => _service.CreateOpeningStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<ServiceException>().WithMessage($"Hardware device of type '{hardwareDevice.Device.Type}' does not support this action");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreateOpeningStateNotification_WhenDeviceIsNotConnected_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var arguments = new ExecuteOpeningStateNotificationArguments("someState");
            var hardwareDevice = new HardwareDevice(_exampleWindowSensor)
            {
                Id = hardwareDeviceId,
                ConnectionState = "disconnected"
            };

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            var act = () => _service.CreateOpeningStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<ServiceException>().WithMessage("Hardware device is not connected");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreateOpeningStateNotification_WhenOpeningStateIsNotOpenOrClosed_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var arguments = new ExecuteOpeningStateNotificationArguments("someState");
            var hardwareDevice = new HardwareDevice(_exampleWindowSensor)
            {
                Id = hardwareDeviceId,
                ConnectionState = "connected",
                OpeningState = "openi"
            };

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            var act = () => _service.CreateOpeningStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<ServiceException>().WithMessage("Opening state must be 'open' or 'closed'");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreateOpeningStateNotification_WhenOpeningStateCurrentlyIsInThatSpecificState_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var arguments = new ExecuteOpeningStateNotificationArguments("open");
            var hardwareDevice = new HardwareDevice(_exampleWindowSensor)
            {
                Id = hardwareDeviceId,
                ConnectionState = "connected",
                OpeningState = "open"
            };

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            var act = () => _service.CreateOpeningStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<ServiceException>().WithMessage("Device is already in the specified state");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreateOpeningStateNotification_WhenChecksArePassed_ShouldReturnResponse()
        {
            var hardwareDeviceId = "someDevice";
            var homeId = "someHomeId";
            var owner = new User { Id = "ownerId" };
            var hardwareDevice = new HardwareDevice(_exampleWindowSensor)
            {
                Id = hardwareDeviceId,
                ConnectionState = "connected",
                Home = new Home() { Id = homeId, Owner = owner }
            };
            var users = new List<User>
            {
                new User { Id = "user1" },
                new User { Id = "user2" }
            };

            var arguments = new ExecuteOpeningStateNotificationArguments("open");

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);
            _homeService.Setup(x => x.GetHomeById(homeId)).Returns(hardwareDevice.Home);
            _systemService.Setup(x => x.GetResidentsByHome(homeId)).Returns(users);
            _homeService.Setup(x => x.DoesUserHaveNotificationPermissionInSpecificHome(It.IsAny<string>(), homeId)).Returns(true);
            _homeService.Setup(x => x.UpdateHardwareDevice(hardwareDevice));
            
            _notificationRepositoryMock.Setup(x => x.Add(It.IsAny<Notification>()));
            _userNotificationRepositoryMock.Setup(x => x.Add(It.IsAny<UserNotification>()));
            
            var response = _service.CreateOpeningStateNotification(hardwareDeviceId, arguments);
            
            response.Should().NotBeNull();
            response.Notification.Should().NotBeNull();
            response.UserNotifications.Should().HaveCount(users.Count + 1);
            
            _homeService.VerifyAll();
            _systemService.VerifyAll();
            _notificationRepositoryMock.VerifyAll();
            _userNotificationRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void CreatePowerStateNotification_WhenHardwareDeviceIsNull_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";
            var arguments = new ExecutePowerStateNotificationArguments("someState");

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns((HardwareDevice)null);

            var act = () => _service.CreatePowerStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<EntityNotFoundException>().WithMessage("Hardware device not found");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePowerStateNotification_WhenHardwareDeviceTypeDoesNotSupportAction_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var arguments = new ExecutePowerStateNotificationArguments("someState");

            var hardwareDevice = new HardwareDevice(_exampleWindowSensor)
            {
                Id = hardwareDeviceId,
                Device = new Device
                {
                    Type = "securityCamera"
                }
            };

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            var act = () => _service.CreatePowerStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<ServiceException>().WithMessage($"Hardware device of type '{hardwareDevice.Device.Type}' does not support this action");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePowerStateNotification_WhenDeviceIsNotConnected_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var arguments = new ExecutePowerStateNotificationArguments("someState");

            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                Device = new Device
                {
                    Type = "smartLamp"
                },
                ConnectionState = "disconnected"
            };

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            var act = () => _service.CreatePowerStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<ServiceException>().WithMessage("Hardware device is not connected");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePowerStateNotification_WhenPowerStateIsNotONorOFF_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var arguments = new ExecutePowerStateNotificationArguments("ONi");

            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                Device = new Device
                {
                    Type = "smartLamp"
                },
                ConnectionState = "connected",
                PowerState = "OFF"
            };

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            var act = () => _service.CreatePowerStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<ServiceException>().WithMessage("Power state must be 'ON' or 'OFF'");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePowerStateNotification_WhenPowerStateCurrentlyIsInThatSpecificState_ShouldThrowException()
        {
            var hardwareDeviceId = "someDevice";
            var arguments = new ExecutePowerStateNotificationArguments("ON");

            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera)
            {
                Id = hardwareDeviceId,
                Device = new Device
                {
                    Type = "smartLamp"
                },
                ConnectionState = "connected",
                PowerState = "ON"
            };

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            var act = () => _service.CreatePowerStateNotification(hardwareDeviceId, arguments);

            act.Should().Throw<ServiceException>().WithMessage("Device is already in the specified state");
            _homeService.VerifyAll();
        }

        [TestMethod]
        public void CreatePowerStateNotification_WhenChecksArePassed_ShouldReturnResponse()
        {
            var hardwareDeviceId = "someDevice";
            var homeId = "someHomeId";
            var owner = new User { Id = "ownerId" };
            var hardwareDevice = new HardwareDevice()
            {
                Id = hardwareDeviceId,
                ConnectionState = "connected",
                PowerState = "OFF",
                Device = new Device { Type = "smartLamp"},
                Home = new Home() { Id = homeId, Owner = owner }
            };
            var users = new List<User>
            {
                new User { Id = "user1" },
                new User { Id = "user2" }
            };

            var arguments = new ExecutePowerStateNotificationArguments("ON");

            _homeService.Setup(x => x.GetHardwareById(It.IsAny<string>(), It.IsAny<bool>())).Returns(hardwareDevice);

            _homeService.Setup(x => x.GetHomeById(homeId)).Returns(hardwareDevice.Home);
            _systemService.Setup(x => x.GetResidentsByHome(homeId)).Returns(users);
            _homeService.Setup(x => x.DoesUserHaveNotificationPermissionInSpecificHome(It.IsAny<string>(), homeId)).Returns(true);
            _homeService.Setup(x => x.UpdateHardwareDevice(hardwareDevice));

            _notificationRepositoryMock.Setup(x => x.Add(It.IsAny<Notification>()));
            _userNotificationRepositoryMock.Setup(x => x.Add(It.IsAny<UserNotification>()));

            var response = _service.CreatePowerStateNotification(hardwareDeviceId, arguments);

            response.Should().NotBeNull();
            response.Notification.Should().NotBeNull();
            response.UserNotifications.Should().HaveCount(users.Count + 1);

            _homeService.VerifyAll();
            _systemService.VerifyAll();
            _notificationRepositoryMock.VerifyAll();
            _userNotificationRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllNotificationsByUser_WhenHasBeenReadIsNotValid_ShouldThrowException()
        {
            var userId = "someUserId";
            var arguments = new GetAllNotificationsArguments("someDeviceType", DateTime.Now, "InvalidValue");

            Action act = () => _service.GetAllNotificationsByUser(userId, arguments);

            act.Should().Throw<ServiceException>().WithMessage("Argument has been read must be 'Yes', 'No' or 'Empty'");
        }

        [TestMethod]
        public void GetAllNotificationsByUser_WhenChecksArePassed_ShouldReturnUserNotifications()
        {
            var userId = "someUserId";
            var deviceType = "Camera";
            var creationDatetime = DateTime.Now;
            var hasBeenRead = "Yes";

            var arguments = new GetAllNotificationsArguments(deviceType, creationDatetime, hasBeenRead);
            var userNotifications = new List<UserNotification>
            {
                new UserNotification
                {
                    Notification = new Notification
                    {
                        Event = "Motion Detected",
                        HardwareDevice = new HardwareDevice(_exampleSecurityCamera)
                        {
                            Id = "deviceId1",
                        },
                        CreationDatetime = creationDatetime
                    },
                    HasBeenRead = true
                }
            };

            _userNotificationRepositoryMock
                .Setup(x => x.GetAll(It.IsAny<Expression<Func<UserNotification, bool>>>()))
                .Returns(userNotifications);

            var result = _service.GetAllNotificationsByUser(userId, arguments);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            _userNotificationRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateHasBeenRead_WhenUserNotificationIsNull_ShouldThrowException()
        {
            var userNotificationId = "someUserNotificationId";
            _userNotificationRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<UserNotification, bool>>>())).Returns((UserNotification)null);
            
            Action act = () => _service.UpdateHasBeenRead(userNotificationId);
            
            act.Should().Throw<EntityNotFoundException>().WithMessage("User notification not found");
            _userNotificationRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void UpdateHasBeenRead_WhenChecksArePassed_ShouldUpdateUserNotification()
        {
            var userNotificationId = "someUserNotificationId";
            var userNotification = new UserNotification { Id = userNotificationId };
            _userNotificationRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<UserNotification, bool>>>())).Returns(userNotification);
            _userNotificationRepositoryMock.Setup(x => x.Update(It.IsAny<UserNotification>()));

            var act = () => _service.UpdateHasBeenRead(userNotificationId);

            act.Should().NotThrow();
            _userNotificationRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Should_Return_Notifications_With_DeviceType_And_CreationDateTime()
        {
            var arguments = new GetAllNotificationsArguments("Camera", DateTime.Now, "");

            var userNotifications = new List<UserNotification>
            {
                new UserNotification()
            };

            _userNotificationRepositoryMock.Setup(repo =>
                repo.GetAll(It.IsAny<Expression<Func<UserNotification, bool>>>()))
                .Returns(userNotifications);
            
            var result = _service.GetAllNotificationsByUser("UserId", arguments);
            
            result.Should().BeEquivalentTo(userNotifications);
            _userNotificationRepositoryMock.Verify(repo =>
                repo.GetAll(It.IsAny<Expression<Func<UserNotification, bool>>>()));
        }

        [TestMethod]
        public void Should_Return_Notifications_With_HasBeenRead_And_CreationDateTime()
        {
            var arguments = new GetAllNotificationsArguments("", DateTime.Now, "Yes");
            var userNotifications = new List<UserNotification>
            {
                new UserNotification()
            };
            
            _userNotificationRepositoryMock.Setup(repo =>
                repo.GetAll(It.IsAny<Expression<Func<UserNotification, bool>>>()))
                .Returns(userNotifications);
            
            var result = _service.GetAllNotificationsByUser("UserId", arguments);
            
            result.Should().BeEquivalentTo(userNotifications);
            _userNotificationRepositoryMock.Verify(repo =>
                repo.GetAll(It.IsAny<Expression<Func<UserNotification, bool>>>()));
            
        }

        [TestMethod]
        public void Should_Return_Notifications_With_CreationDateTime()
        {
            var arguments = new GetAllNotificationsArguments("", DateTime.Now, "");
            var userNotifications = new List<UserNotification>
            {
                new UserNotification()
            };
            
            _userNotificationRepositoryMock.Setup(repo =>
                repo.GetAll(It.IsAny<Expression<Func<UserNotification, bool>>>()))
                .Returns(userNotifications);
            
            var result = _service.GetAllNotificationsByUser("UserId", arguments);
            
            result.Should().BeEquivalentTo(userNotifications);
            _userNotificationRepositoryMock.Verify(repo =>
                repo.GetAll(It.IsAny<Expression<Func<UserNotification, bool>>>()));
        }
        
        [TestMethod]
        public void UserNotification_Id_Should_Get_And_Set_Value()
        {
            var userNotification = new UserNotification();
            var id = "testId";

            userNotification.Id = id;

            userNotification.Id.Should().Be(id);
        }

        [TestMethod]
        public void UserNotification_UserId_Should_Get_And_Set_Value()
        {
            var userNotification = new UserNotification();
            var userId = "testUserId";

            userNotification.UserId = userId;

            userNotification.UserId.Should().Be(userId);
        }

        [TestMethod]
        public void UserNotification_User_Should_Get_And_Set_Value()
        {
            var userNotification = new UserNotification();
            var user = new User { Id = "userId" };

            userNotification.User = user;

            userNotification.User.Should().Be(user);
        }

        [TestMethod]
        public void UserNotification_NotificationId_Should_Get_And_Set_Value()
        {
            var userNotification = new UserNotification();
            var notificationId = "testNotificationId";

            userNotification.NotificationId = notificationId;

            userNotification.NotificationId.Should().Be(notificationId);
        }

        [TestMethod]
        public void UserNotification_Notification_Should_Get_And_Set_Value()
        {
            var userNotification = new UserNotification();
            var notification = new Notification { Id = "notificationId" };

            userNotification.Notification = notification;

            userNotification.Notification.Should().Be(notification);
        }

        [TestMethod]
        public void UserNotification_HasBeenRead_Should_Get_And_Set_Value()
        {
            var userNotification = new UserNotification();
            var hasBeenRead = true;

            userNotification.HasBeenRead = hasBeenRead;

            userNotification.HasBeenRead.Should().Be(hasBeenRead);
        }
        
        [TestMethod]
        public void HardwareDevice_Id_Should_Get_And_Set_Value()
        {
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera);
            var id = "testId";

            hardwareDevice.Id = id;

            hardwareDevice.Id.Should().Be(id);
        }

        [TestMethod]
        public void HardwareDevice_DeviceId_Should_Get_And_Set_Value()
        {
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera);
            var deviceId = "testDeviceId";

            hardwareDevice.DeviceId = deviceId;

            hardwareDevice.DeviceId.Should().Be(deviceId);
        }

        [TestMethod]
        public void HardwareDevice_Device_Should_Get_And_Set_Value()
        {
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera);

            hardwareDevice.Device = _exampleSecurityCamera;

            hardwareDevice.Device.Should().Be(_exampleSecurityCamera);
        }

        [TestMethod]
        public void HardwareDevice_HomeId_Should_Get_And_Set_Value()
        {
            var hardwareDevice = new HardwareDevice(_exampleWindowSensor);
            var homeId = "testHomeId";

            hardwareDevice.HomeId = homeId;

            hardwareDevice.HomeId.Should().Be(homeId);
        }

        [TestMethod]
        public void HardwareDevice_Home_Should_Get_And_Set_Value()
        {
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera);
            var home = new Home { Id = "homeId", Address = new Address("18 de Julio", 237) };

            hardwareDevice.Home = home;

            hardwareDevice.Home.Should().Be(home);
        }

        [TestMethod]
        public void HardwareDevice_ConnectionState_Should_Get_And_Set_Value()
        {
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera);
            var connectionState = "disconnected";

            hardwareDevice.ConnectionState = connectionState;

            hardwareDevice.ConnectionState.Should().Be(connectionState);
        }

        [TestMethod]
        public void HardwareDevice_OpeningState_Should_Get_And_Set_Value()
        {
            var hardwareDevice = new HardwareDevice(_exampleWindowSensor);
            var openingState = "open";

            hardwareDevice.OpeningState = openingState;

            hardwareDevice.OpeningState.Should().Be(openingState);
        }
        
        [TestMethod]
        public void Notification_Id_Should_Get_And_Set_Value()
        {
            var notification = new Notification();
            var id = Guid.NewGuid().ToString();

            notification.Id = id;

            notification.Id.Should().Be(id);
        }

        [TestMethod]
        public void Notification_Event_Should_Get_And_Set_Value()
        {
            var notification = new Notification();
            var eventDescription = "Motion Detected";

            notification.Event = eventDescription;

            notification.Event.Should().Be(eventDescription);
        }

        [TestMethod]
        public void Notification_HardwareDevice_Should_Get_And_Set_Value()
        {
            var notification = new Notification();
            var hardwareDevice = new HardwareDevice(_exampleSecurityCamera) { Id = "hardwareDeviceId" };

            notification.HardwareDevice = hardwareDevice;

            notification.HardwareDevice.Should().Be(hardwareDevice);
        }

        [TestMethod]
        public void Notification_CreationDatetime_Should_Get_And_Set_Value()
        {
            var notification = new Notification();
            var creationDatetime = DateTime.UtcNow;

            notification.CreationDatetime = creationDatetime;

            notification.CreationDatetime.Should().Be(creationDatetime);
        }
    }
}