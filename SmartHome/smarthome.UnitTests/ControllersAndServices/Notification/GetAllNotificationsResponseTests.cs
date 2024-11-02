using Domain.NotificationModels;
using FluentAssertions;

namespace smarthome.UnitTests;

[TestClass]
public class GetAllNotificationsResponseTests
{
    [TestMethod]
        public void GetAllNotificationsResponse_Event_Should_Get_And_Set_Value()
        {
            var response = new GetAllNotificationsResponse
            {
                Event = "Motion Detected"
            };

            response.Event.Should().Be("Motion Detected");
        }

        [TestMethod]
        public void GetAllNotificationsResponse_HardwareDeviceId_Should_Get_And_Set_Value()
        {
            var response = new GetAllNotificationsResponse
            {
                HardwareDeviceId = "hardwareDeviceId123"
            };

            response.HardwareDeviceId.Should().Be("hardwareDeviceId123");
        }

        [TestMethod]
        public void GetAllNotificationsResponse_CreationDatetime_Should_Get_And_Set_Value()
        {
            var creationDate = new DateTime(2024, 10, 6);
            var response = new GetAllNotificationsResponse
            {
                CreationDatetime = creationDate
            };

            response.CreationDatetime.Should().Be(creationDate);
        }

        [TestMethod]
        public void GetAllNotificationsResponse_DeviceName_Should_Get_And_Set_Value()
        {
            var response = new GetAllNotificationsResponse
            {
                DeviceName = "Security Camera"
            };

            response.DeviceName.Should().Be("Security Camera");
        }

        [TestMethod]
        public void GetAllNotificationsResponse_DeviceModelNumber_Should_Get_And_Set_Value()
        {
            var response = new GetAllNotificationsResponse
            {
                DeviceModelNumber = "Model123"
            };

            response.DeviceModelNumber.Should().Be("Model123");
        }

        [TestMethod]
        public void GetAllNotificationsResponse_HasBeenRead_Should_Get_And_Set_Value()
        {
            var response = new GetAllNotificationsResponse
            {
                HasBeenRead = true
            };

            response.HasBeenRead.Should().BeTrue();
        }

        [TestMethod]
        public void GetAllNotificationsResponse_Should_Initialize_Properties_Correctly()
        {
            var eventDescription = "Motion Detected";
            var hardwareDeviceId = "hardwareDeviceId123";
            var creationDate = new DateTime(2024, 10, 6);
            var deviceName = "Security Camera";
            var deviceModelNumber = "Model123";
            var hasBeenRead = true;

            var response = new GetAllNotificationsResponse
            {
                Event = eventDescription,
                HardwareDeviceId = hardwareDeviceId,
                CreationDatetime = creationDate,
                DeviceName = deviceName,
                DeviceModelNumber = deviceModelNumber,
                HasBeenRead = hasBeenRead
            };

            response.Event.Should().Be(eventDescription);
            response.HardwareDeviceId.Should().Be(hardwareDeviceId);
            response.CreationDatetime.Should().Be(creationDate);
            response.DeviceName.Should().Be(deviceName);
            response.DeviceModelNumber.Should().Be(deviceModelNumber);
            response.HasBeenRead.Should().Be(hasBeenRead);
        }
}