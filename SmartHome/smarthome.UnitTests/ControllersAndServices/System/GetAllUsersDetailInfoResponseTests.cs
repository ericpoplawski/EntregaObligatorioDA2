using Domain.UserModels;
using FluentAssertions;

namespace smarthome.UnitTests;

[TestClass]
public class GetAllUsersDetailInfoResponseTests
{
    [TestMethod]
    public void GetAllUsersDetailInfoResponse_FirstName_Should_Get_And_Set_Value()
    {
        var response = new GetAllUsersDetailInfoResponse();
        var firstName = "John";

        response.FirstName = firstName;

        response.FirstName.Should().Be(firstName);
    }

    [TestMethod]
    public void GetAllUsersDetailInfoResponse_LastName_Should_Get_And_Set_Value()
    {
        var response = new GetAllUsersDetailInfoResponse();
        var lastName = "Doe";

        response.LastName = lastName;

        response.LastName.Should().Be(lastName);
    }

    [TestMethod]
    public void GetAllUsersDetailInfoResponse_FullName_Should_Get_And_Set_Value()
    {
        var response = new GetAllUsersDetailInfoResponse();
        var fullName = "John Doe";

        response.FullName = fullName;

        response.FullName.Should().Be(fullName);
    }

    [TestMethod]
    public void GetAllUsersDetailInfoResponse_CreationDate_Should_Get_And_Set_Value()
    {
        var response = new GetAllUsersDetailInfoResponse();
        var creationDate = new DateTime(2024, 10, 6);

        response.CreationDate = creationDate;

        response.CreationDate.Should().Be(creationDate);
    }
        
    [TestMethod]
    public void GetAllUsersDetailInfoResponse_Should_Initialize_Properties_Correctly()
    {
        var firstName = "John";
        var lastName = "Doe";
        var fullName = "John Doe";
        var roleName = "Administrator";
        var creationDate = new DateTime(2024, 10, 6);

        var response = new GetAllUsersDetailInfoResponse
        {
            FirstName = firstName,
            LastName = lastName,
            FullName = fullName,
            RoleNames = new List<string> { roleName },
            CreationDate = creationDate
        };

        response.FirstName.Should().Be(firstName);
        response.LastName.Should().Be(lastName);
        response.FullName.Should().Be(fullName);
        response.RoleNames[0].Should().Be(roleName);
        response.CreationDate.Should().Be(creationDate);
    }
}