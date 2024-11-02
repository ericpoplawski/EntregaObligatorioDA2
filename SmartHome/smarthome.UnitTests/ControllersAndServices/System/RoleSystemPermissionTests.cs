using Domain;
using FluentAssertions;
using smarthome.DataAccess;

namespace smarthome.UnitTests;

[TestClass]
public class RoleSystemPermissionTests
{
    [TestMethod]
    public void RoleId_GetSet_ReturnsExpectedValue()
    {
        var expectedValue = "testRoleId";
        var roleSystemPermission = new RoleSystemPermission();
        
        roleSystemPermission.RoleId = expectedValue;
        var actualValue = roleSystemPermission.RoleId;
        
        actualValue.Should().Be(expectedValue);
    }

    [TestMethod]
    public void SystemPermissionId_GetSet_ReturnsExpectedValue()
    {
        var expectedValue = "testPermissionId";
        var roleSystemPermission = new RoleSystemPermission();
        
        roleSystemPermission.SystemPermissionId = expectedValue;
        var actualValue = roleSystemPermission.SystemPermissionId;
        
        actualValue.Should().Be(expectedValue);
    }

    [TestMethod]
    public void Role_GetSet_ReturnsExpectedValue()
    {
        var expectedValue = new Role();
        var roleSystemPermission = new RoleSystemPermission();
        
        roleSystemPermission.Role = expectedValue;
        var actualValue = roleSystemPermission.Role;
        
        actualValue.Should().Be(expectedValue);
    }

    [TestMethod]
    public void Permission_GetSet_ReturnsExpectedValue()
    {
        var expectedValue = new SystemPermission();
        var roleSystemPermission = new RoleSystemPermission();
        
        roleSystemPermission.Permission = expectedValue;
        var actualValue = roleSystemPermission.Permission;
        
        actualValue.Should().Be(expectedValue);
    }
}