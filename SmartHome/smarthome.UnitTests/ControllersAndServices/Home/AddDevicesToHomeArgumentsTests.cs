using Domain.HomeModels;
using FluentAssertions;

namespace smarthome.UnitTests;

[TestClass]
public class AddDevicesToHomeArgumentsTests
{
    [TestMethod]
    public void AddDeviceToHomeArguments_HomeId_Should_Return_Initialized_Value()
    {
        var homeId = "testHomeId";
        var deviceId = "testDeviceId";
        var roomId = "testRoomId";

        var arguments = new AddDeviceToHomeArguments(homeId, roomId, deviceId);
            
        arguments.HomeId.Should().Be(homeId);
    }

    [TestMethod]
    public void AddDeviceToHomeArguments_DeviceId_Should_Return_Initialized_Value()
    {
        var homeId = "testHomeId";
        var deviceId = "testDeviceId";
        var roomId = "testRoomId";

        var arguments = new AddDeviceToHomeArguments(homeId, roomId, deviceId);

        arguments.DeviceId.Should().Be(deviceId);
    }

    [TestMethod]
    public void AddDeviceToHomeArguments_Constructor_Should_Initialize_Properties_Correctly()
    {
        var homeId = "testHomeId";
        var deviceId = "testDeviceId";
        var roomId = "testRoomId";

        var arguments = new AddDeviceToHomeArguments(homeId, roomId, deviceId);

        arguments.HomeId.Should().Be(homeId);
        arguments.DeviceId.Should().Be(deviceId);
    }
}