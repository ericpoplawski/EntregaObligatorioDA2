using CQ.Utility;

namespace Domain.HomeModels;

public class ConfigureResidentsHomePermissionsArguments
{
    public string UserId { get; set; }
    public string HomePermission { get; set; }
    
    public ConfigureResidentsHomePermissionsArguments(string userId, string homePermission)
    {
        Guard.ThrowIsNullOrEmpty(userId, nameof(userId));
        UserId = userId;
        HomePermission = homePermission;
    }
}