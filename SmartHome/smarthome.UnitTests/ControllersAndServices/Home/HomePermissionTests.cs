using Domain;
using FluentAssertions;

namespace smarthome.UnitTests;

[TestClass]
public class HomePermissionTests
{
    [TestMethod]
    public void HomePermission_Id_Should_Get_And_Set_Value()
    {
        var homePermission = new HomePermission();
        var id = Guid.NewGuid().ToString();

        homePermission.Id = id;

        homePermission.Id.Should().Be(id);
    }

    [TestMethod]
    public void HomePermission_Name_Should_Get_And_Set_Value()
    {
        var homePermission = new HomePermission();
        var name = "CanViewDevices";

        homePermission.Name = name;

        homePermission.Name.Should().Be(name);
    }

    [TestMethod]
    public void HomePermission_Constructor_Should_Initialize_Id()
    {
        var homePermission = new HomePermission();

        homePermission.Id.Should().NotBeNullOrEmpty();
    }
    
}