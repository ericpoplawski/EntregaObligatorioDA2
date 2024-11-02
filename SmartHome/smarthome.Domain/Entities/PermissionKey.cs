namespace Domain
{
    public sealed record class PermissionKey
    {
        public readonly static PermissionKey CreateCompany = new("CreateCompany");
        public readonly static PermissionKey RegisterSecurityCamera = new("RegisterSecurityCamera");
        public readonly static PermissionKey CreateCompanyOwner = new("CreateCompanyOwner");

        public readonly static PermissionKey ListUsers = new("ListUsers");
        public readonly static PermissionKey ListCompanies = new("ListCompanies");
        public readonly static PermissionKey ListDevices = new("ListDevices");
        public readonly static PermissionKey ListSupportedDevices = new("ListSupportedDevices");
        public readonly static PermissionKey CreateAdministrator = new("CreateAdministrator");
        public readonly static PermissionKey RegisterWindowSensor = new("RegisterWindowSensor");
        public readonly static PermissionKey CreateHome = new("CreateHome");
        public readonly static PermissionKey AddResidentToHome = new("AddResidentToHome");
        public readonly static PermissionKey BindDeviceToHome = new("BindDeviceToHome");
        public readonly static PermissionKey BindRoomToHome = new("BindRoomToHome");
        public readonly static PermissionKey ListHomeResidents = new("ListHomeResidents");
        public readonly static PermissionKey ListHomeDevices = new("ListHomeDevices");
        public readonly static PermissionKey ConfigureResidentsPermissions = new("ConfigureResidentsPermissions");
        public readonly static PermissionKey ListNotifications = new("ListNotifications");
        public readonly static PermissionKey ConfigureNotificationPermission = new("ConfigureNotificationPermission");
        public readonly static PermissionKey CanReceiveNotifications = new("DoesResidentCanReceiveNotifications");
        public readonly static PermissionKey ChangeHardwareDeviceName = new("ChangeHardwareDeviceName");


        public readonly static PermissionKey DeleteAdministrator = new("DeleteAdministrator");
        public readonly static PermissionKey AddHomeOwnerRoleToUser = new("AddHomeOwnerRoleToUser");
        public readonly static PermissionKey RegisterMotionSensor = new("RegisterMotionSensor");

        private readonly string Value;
        public PermissionKey(string value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value;
        }
    }
}