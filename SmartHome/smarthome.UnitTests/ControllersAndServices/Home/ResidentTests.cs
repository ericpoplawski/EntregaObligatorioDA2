using Domain;
using FluentAssertions;

namespace smarthome.UnitTests;

[TestClass]
public class ResidentTests
{
    [TestMethod]
        public void Resident_Id_Should_Get_And_Set_Value()
        {
            var resident = new Resident();
            var id = Guid.NewGuid().ToString();

            resident.Id = id;

            resident.Id.Should().Be(id);
        }

        [TestMethod]
        public void Resident_HomeId_Should_Get_And_Set_Value()
        {
            var resident = new Resident();
            var homeId = "testHomeId";

            resident.HomeId = homeId;

            resident.HomeId.Should().Be(homeId);
        }

        [TestMethod]
        public void Resident_Home_Should_Get_And_Set_Value()
        {
            var resident = new Resident();
            var home = new Home { Id = "homeId" };

            resident.Home = home;

            resident.Home.Should().Be(home);
        }

        [TestMethod]
        public void Resident_HomePermissions_Should_Get_And_Set_Value()
        {
            var resident = new Resident();
            var homePermissions = new List<HomePermission>
            {
                new HomePermission { Name = "Permission1" },
                new HomePermission { Name = "Permission2" }
            };

            resident.HomePermissions = homePermissions;

            resident.HomePermissions.Should().BeEquivalentTo(homePermissions);
        }

        [TestMethod]
        public void Resident_Constructor_Should_Initialize_Properties_Correctly()
        {
            var home = new Home { Id = "homeId" };
            var homePermissions = new List<HomePermission>
            {
                new HomePermission { Name = "Permission1" },
                new HomePermission { Name = "Permission2" }
            };

            var resident = new Resident(home, homePermissions);

            resident.Id.Should().NotBeNullOrEmpty();
            resident.Home.Should().Be(home);
            resident.HomeId.Should().Be(home.Id);
            resident.HomePermissions.Should().BeEquivalentTo(homePermissions);
        }

        [TestMethod]
        public void Resident_Default_Constructor_Should_Initialize_HomePermissions_As_Empty_List()
        {
            var resident = new Resident();

            resident.HomePermissions.Should().NotBeNull();
            resident.HomePermissions.Should().BeEmpty();
        }
}