using Domain.HomeModels;
using FluentAssertions;

namespace smarthome.UnitTests;

[TestClass]
public class GetHardwareDevicesDaetailInfoResponseTests
{
    [TestMethod]
    public void GetHardwareDevicesDetailInfoResponse_DeviceName_Should_Return_Initialized_Value()
    {
        var deviceName = "Smart Camera";
        var deviceModelNumber = "SC123";
        var mainPicture = "http://example.com/image.jpg";
        var connectionState = "connected";

        var response = new GetHardwareDevicesDetailInfoResponse
        {
            DeviceName = deviceName,
            DeviceModelNumber = deviceModelNumber,
            MainPicture = mainPicture,
            ConnectionState = connectionState
        };

        response.DeviceName.Should().Be(deviceName);
    }

    [TestMethod]
    public void GetHardwareDevicesDetailInfoResponse_DeviceModelNumber_Should_Return_Initialized_Value()
    {
        var deviceName = "Smart Camera";
        var deviceModelNumber = "SC123";
        var mainPicture = "http://example.com/image.jpg";
        var connectionState = "connected";

        var response = new GetHardwareDevicesDetailInfoResponse
        {
            DeviceName = deviceName,
            DeviceModelNumber = deviceModelNumber,
            MainPicture = mainPicture,
            ConnectionState = connectionState
        };

        response.DeviceModelNumber.Should().Be(deviceModelNumber);
    }

    [TestMethod]
    public void GetHardwareDevicesDetailInfoResponse_MainPicture_Should_Return_Initialized_Value()
    {
        var deviceName = "Smart Camera";
        var deviceModelNumber = "SC123";
        var mainPicture = "http://example.com/image.jpg";
        var connectionState = "connected";

        var response = new GetHardwareDevicesDetailInfoResponse
        {
            DeviceName = deviceName,
            DeviceModelNumber = deviceModelNumber,
            MainPicture = mainPicture,
            ConnectionState = connectionState
        };

        response.MainPicture.Should().Be(mainPicture);
    }

    [TestMethod]
    public void GetHardwareDevicesDetailInfoResponse_ConnectionState_Should_Return_Initialized_Value()
    {
        var deviceName = "Smart Camera";
        var deviceModelNumber = "SC123";
        var mainPicture = "http://example.com/image.jpg";
        var connectionState = "connected";

        var response = new GetHardwareDevicesDetailInfoResponse
        {
            DeviceName = deviceName,
            DeviceModelNumber = deviceModelNumber,
            MainPicture = mainPicture,
            ConnectionState = connectionState
        };

        response.ConnectionState.Should().Be(connectionState);
    }

    [TestMethod]
    public void GetHardwareDevicesDetailInfoResponse_Constructor_Should_Initialize_Properties_Correctly()
    {
        var deviceName = "Smart Camera";
        var deviceModelNumber = "SC123";
        var mainPicture = "http://example.com/image.jpg";
        var connectionState = "connected";

        var response = new GetHardwareDevicesDetailInfoResponse
        {
            DeviceName = deviceName,
            DeviceModelNumber = deviceModelNumber,
            MainPicture = mainPicture,
            ConnectionState = connectionState
        };

        response.DeviceName.Should().Be(deviceName);
        response.DeviceModelNumber.Should().Be(deviceModelNumber);
        response.MainPicture.Should().Be(mainPicture);
        response.ConnectionState.Should().Be(connectionState);
    }
}