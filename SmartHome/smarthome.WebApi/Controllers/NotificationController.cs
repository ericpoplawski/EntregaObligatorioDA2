using Domain;
using Domain.Exceptions;
using Domain.NotificationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smarthome.BussinessLogic.Services.Notifications;
using smarthome.WebApi.Filters;

namespace smarthome.WebApi.Controllers
{
    [ApiController]
    [AuthenticationFilter]
    public class NotificationController : SmartHomeControllerBase
    {
        private readonly INotificationService _service;
        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/devices/{hardwareDeviceId}/motionDetectionNotifications")]
        [AllowAnonymous]
        public void ExecuteMotionDetectionNotification([FromRoute]string hardwareDeviceId)
        {
            _service.CreateMotionDetectionNotification(hardwareDeviceId);
        }

        [HttpPost]
        [Route("api/devices/{hardwareDeviceId}/personDetectionNotifications")]
        [AllowAnonymous]
        public void ExecutePersonDetectionNotification([FromRoute] string hardwareDeviceId)
        {
            _service.CreatePersonDetectionNotification(hardwareDeviceId);
        }

        [HttpPost]
        [Route("api/devices/{hardwareDeviceId}/openingStateNotifications")]
        [AllowAnonymous]
        public void ExecuteOpeningStateNotification([FromRoute] string hardwareDeviceId,
            ExecuteOpeningStateNotificationRequest request)
        {
            if (request == null)
            {
                throw new ControllerException("Request is null");
            }

            ExecuteOpeningStateNotificationArguments arguments 
                = new ExecuteOpeningStateNotificationArguments(request.NewOpeningState);

            _service.CreateOpeningStateNotification(hardwareDeviceId, arguments);
        }

        [HttpPost]
        [Route("api/devices/{hardwareDeviceId}/powerStateNotifications")]
        [AllowAnonymous]
        public void ExecutePowerStateNotification([FromRoute] string hardwareDeviceId,
            ExecutePowerStateNotificationRequest request)
        {
            if (request == null)
            {
                throw new ControllerException("Request is null");
            }
            ExecutePowerStateNotificationArguments arguments
                = new ExecutePowerStateNotificationArguments(request.NewPowerState);

            _service.CreatePowerStateNotification(hardwareDeviceId, arguments);
        }

            [HttpGet]
        [Route("api/users/{userId}/notifications")]
        [AuthorizationFilter("ListNotifications")]
        public List<GetAllNotificationsResponse> ListNotificationsForUser([FromRoute] string userId, 
            [FromQuery] string deviceType, [FromQuery] DateTime creationDatetime, [FromQuery] string hasBeenRead)
        {
            var arguments = new GetAllNotificationsArguments(deviceType, creationDatetime, hasBeenRead);
            List<UserNotification> userNotifications = _service.GetAllNotificationsByUser(userId, arguments);
            List<GetAllNotificationsResponse> responses = new List<GetAllNotificationsResponse>();

            foreach (var userNotification in userNotifications)
            {
                GetAllNotificationsResponse response = new GetAllNotificationsResponse()
                {
                    Event = userNotification.Notification.Event,
                    HardwareDeviceId = userNotification.Notification.HardwareDevice.Id,
                    CreationDatetime = userNotification.Notification.CreationDatetime,
                    DeviceName = userNotification.Notification.HardwareDevice.Device.Name,
                    DeviceModelNumber = userNotification.Notification.HardwareDevice.Device.ModelNumber,
                    HasBeenRead = userNotification.HasBeenRead
                };
                responses.Add(response);

                _service.UpdateHasBeenRead(userNotification.Id);
            }
            return responses;
        }
    }
}