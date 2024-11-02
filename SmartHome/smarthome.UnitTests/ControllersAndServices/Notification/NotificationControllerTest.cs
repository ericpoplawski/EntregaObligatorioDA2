using Domain;
using Domain.Exceptions;
using Domain.NotificationModels;
using FluentAssertions;
using Moq;
using smarthome.BussinessLogic.Services.Notifications;
using smarthome.WebApi.Controllers;

namespace smarthome.UnitTests.NotificationTests
{
    [TestClass]
    public class NotificationControllerTest
    {
        private NotificationController _controller;
        private Mock<INotificationService> _notificationServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            _notificationServiceMock = new Mock<INotificationService>(MockBehavior.Strict);
            _controller = new NotificationController(_notificationServiceMock.Object);
        }

        [TestMethod]
        public void ExecuteMotionDetectionNotification_ShouldThrowOk()
        {
            var hardwareDeviceId = "someDeviceId";
            var notification = new Notification();
            var userNotifications = new List<UserNotification>();
            var notificationResponse = new CreateNotificationResponse(notification, userNotifications);
            _notificationServiceMock.Setup(x => x.CreateMotionDetectionNotification(hardwareDeviceId)).Returns(notificationResponse);

            var act = () => _controller.ExecuteMotionDetectionNotification(hardwareDeviceId);

            act.Should().NotThrow();
            _notificationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ExecutePersonDetectionNotification_ShouldThrowOk()
        {
            var hardwareDeviceId = "someDeviceId";
            var notification = new Notification();
            var userNotifications = new List<UserNotification>();
            var notificationResponse = new CreateNotificationResponse(notification, userNotifications);
            _notificationServiceMock.Setup(x => x.CreatePersonDetectionNotification(hardwareDeviceId)).Returns(notificationResponse);
            
            var act = () => _controller.ExecutePersonDetectionNotification(hardwareDeviceId);

            act.Should().NotThrow();
            _notificationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ExecuteOpeningStateNotification_WhenRequestIsNull_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";

            var act = () => _controller.ExecuteOpeningStateNotification(hardwareDeviceId, null);

            act.Should().Throw<ControllerException>().WithMessage("Request is null");
            _notificationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ExecuteOpeningStateNotification_WhenNewOpeningStateIsNull_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";
            var request = new ExecuteOpeningStateNotificationRequest()
            {
                NewOpeningState = null
            };

            var act = () => _controller.ExecuteOpeningStateNotification(hardwareDeviceId, request);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'newOpeningState')");
            _notificationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ExecuteOpeningStateNotification_ShouldThrowOk()
        {
            var hardwareDeviceId = "someDeviceId";
            var notification = new Notification();
            var userNotifications = new List<UserNotification>();
            var notificationResponse = new CreateNotificationResponse(notification, userNotifications);

            _notificationServiceMock
                .Setup(x => x.CreateOpeningStateNotification(hardwareDeviceId, It.Is<ExecuteOpeningStateNotificationArguments>(arg => arg.NewOpeningState == "someState")))
                .Returns(notificationResponse);
            
            var act = () => _controller.ExecuteOpeningStateNotification(hardwareDeviceId, new ExecuteOpeningStateNotificationRequest { NewOpeningState = "someState" });

            act.Should().NotThrow();
            _notificationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ExecutePowerStateNotification_WhenRequestIsNull_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";

            var act = () => _controller.ExecutePowerStateNotification(hardwareDeviceId, null);

            act.Should().Throw<ControllerException>().WithMessage("Request is null");
        }

        [TestMethod]
        public void ExecutePowerStateNotification_WhenNewOpeningStateIsNull_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";
            var request = new ExecutePowerStateNotificationRequest()
            {
                NewPowerState = null
            };

            var act = () => _controller.ExecutePowerStateNotification(hardwareDeviceId, request);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'newPowerState')");
        }

        [TestMethod]
        public void ExecutePowerStateNotification_WhenNewOpeningStateIsEmpty_ShouldThrowException()
        {
            var hardwareDeviceId = "someDeviceId";
            var request = new ExecutePowerStateNotificationRequest()
            {
                NewPowerState = ""
            };

            var act = () => _controller.ExecutePowerStateNotification(hardwareDeviceId, request);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value of parameter cannot be null or empty (Parameter 'newPowerState')");
        }

        [TestMethod]
        public void ExecutePowerStateNotification_WhenAllChecksArePassed_ShouldThrowOk()
        {
            var hardwareDeviceId = "someDeviceId";

            var notification = new Notification();
            var userNotifications = new List<UserNotification>();
            var notificationResponse = new CreateNotificationResponse(notification, userNotifications);

            _notificationServiceMock
                .Setup(x => x.CreatePowerStateNotification(hardwareDeviceId, It.Is<ExecutePowerStateNotificationArguments>(arg => arg.NewPowerState == "someState")))
                .Returns(notificationResponse);

            var act = () => _controller.ExecutePowerStateNotification(hardwareDeviceId, 
                new ExecutePowerStateNotificationRequest { NewPowerState = "someState" });

            act.Should().NotThrow();
            _notificationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ListNotifications_ShouldReturnNotificationsResponse()
        {
            var userId = "someUserId";
            var deviceType = "Camera";
            var creationDatetime = DateTime.Now;
            var hasBeenRead = "Yes";

            var arguments = new GetAllNotificationsArguments(deviceType, creationDatetime, hasBeenRead);
            var device = new Device
            {
                Name = "Camera",
                ModelNumber = "Model123"
            };
            var userNotifications = new List<UserNotification>
            {
                new UserNotification
                {
                    Notification = new Notification
                    {
                        Event = "Motion Detected",
                        HardwareDevice = new HardwareDevice(device)
                        {
                            Id = "deviceId1"
                        },
                        CreationDatetime = creationDatetime
                    },
                    HasBeenRead = true
                }
            };

            _notificationServiceMock
                .Setup(x => x.GetAllNotificationsByUser(userId, It.Is<GetAllNotificationsArguments>(a =>
                    a.DeviceType == deviceType &&
                    a.CreationDatetime == creationDatetime &&
                    a.HasBeenRead == hasBeenRead)))
                .Returns(userNotifications);

            var userNotification = new UserNotification();
            _notificationServiceMock.Setup(x => x.UpdateHasBeenRead(It.IsAny<string>())).Returns(userNotification);

            var result = _controller.ListNotificationsForUser(userId, deviceType, creationDatetime, hasBeenRead);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Event.Should().Be("Motion Detected");
            result[0].HardwareDeviceId.Should().Be("deviceId1");
            result[0].DeviceName.Should().Be("Camera");
            result[0].DeviceModelNumber.Should().Be("Model123");
            result[0].HasBeenRead.Should().BeTrue();

            _notificationServiceMock.VerifyAll();
        }
    }
}