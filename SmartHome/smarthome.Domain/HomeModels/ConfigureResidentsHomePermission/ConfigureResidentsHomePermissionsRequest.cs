namespace Domain.HomeModels;

public sealed record class ConfigureResidentsHomePermissionsRequest
{
    public string UserId { get; set; }
    public string HomePermissionName { get; set; }
}