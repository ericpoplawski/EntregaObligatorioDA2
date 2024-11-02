using System.Diagnostics.CodeAnalysis;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace smarthome.DataAccess
{
    [ExcludeFromCodeCoverage]
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<HardwareDevice> HardwareDevices { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SystemPermission> SystemPermissions { get; set; }
        public DbSet<HomePermission> HomePermissions { get; set; }
        public DbSet<Resident> Memberships { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany()
                .UsingEntity<UserRole>(
                    j => j.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId),
                    j => j.HasOne(ur => ur.User).WithMany().HasForeignKey(ur => ur.UserId));

            
            modelBuilder.Entity<Resident>()
                .HasOne(m => m.Home)
                .WithMany()
                .HasForeignKey(m => m.HomeId);
            
            modelBuilder.Entity<Home>()
                .OwnsOne(h => h.Address);
            
            modelBuilder.Entity<Home>()
                .OwnsOne(h => h.Location);
            
            modelBuilder.Entity<Home>()
                .HasOne(h => h.Owner)
                .WithMany()
                .HasForeignKey(h => h.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Device>()
                .HasOne(d => d.Company)
                .WithMany()
                .HasForeignKey(d => d.CompanyId);
            
            modelBuilder.Entity<HardwareDevice>()
                .HasOne(hd => hd.Device)
                .WithMany()
                //.WithOne()
                .HasForeignKey(hd => hd.DeviceId);
            
            modelBuilder.Entity<HardwareDevice>()
                .HasOne(hd => hd.Home)
                .WithMany()
                .HasForeignKey(hd => hd.HomeId);
            
            modelBuilder.Entity<UserNotification>()
                .HasOne(un => un.Notification)
                .WithMany()
                .HasForeignKey(un => un.NotificationId);

            modelBuilder.Entity<UserNotification>()
                .HasOne(un => un.User)
                .WithMany()
                .HasForeignKey(un => un.UserId);
            
            modelBuilder.Entity<Resident>()
                .HasMany(m => m.HomePermissions)
                .WithMany();

            modelBuilder.Entity<Role>()
                .HasMany(r => r.SystemPermissions)
                .WithMany(sp => sp.Roles)  
                .UsingEntity(j => j.ToTable("RoleSystemPermissions"));
            
            modelBuilder.Entity<Session>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);
            ConfigSeedData(modelBuilder);
                
                
        }
        
        private void ConfigSeedData(ModelBuilder modelBuilder)
        {

            var roleBuilder = modelBuilder
                .Entity<Role>()
                .HasMany(a => a.SystemPermissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RoleSystemPermission>(
                r => r.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.SystemPermissionId),
                l => l.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId));

            var AdministratorRole = new Role
            {
                Id = Guid.NewGuid().ToString(),
                RoleName = RoleKey.Administrator.ToString(),
                SystemPermissions = new List<SystemPermission>()
            };

            var CompanyOwnerRole = new Role
            {
                Id = Guid.NewGuid().ToString(),
                RoleName = RoleKey.CompanyOwner.ToString(),
                SystemPermissions = new List<SystemPermission>()
            };

            var HomeOwnerRole = new Role
            {
                Id = Guid.NewGuid().ToString(),
                RoleName = RoleKey.HomeOwner.ToString(),
                SystemPermissions = new List<SystemPermission>()
            };

            var CreateAdministrator = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.CreateAdministrator.ToString()
            };

            var CreateCompanyOwner = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.CreateCompanyOwner.ToString()
            };

            var RegisterSecurityCamera = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.RegisterSecurityCamera.ToString()
            };

            var ListUsers = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ListUsers.ToString()
            };

            var ListCompanies = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ListCompanies.ToString()
            };

            var ListDevices = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ListDevices.ToString()
            };

            var ListSupportedDevices = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ListSupportedDevices.ToString()
            };

            var CreateCompany = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.CreateCompany.ToString()
            };

            var RegisterWindowSensor = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.RegisterWindowSensor.ToString()
            };

            var CreateHome = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.CreateHome.ToString()
            };

            var AddResidentToHome = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.AddResidentToHome.ToString()
            };

            var BindDeviceToHome = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.BindDeviceToHome.ToString()
            };

            var ListHomeResidents = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ListHomeResidents.ToString()
            };

            var ListHomeDevices = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ListHomeDevices.ToString()
            };

            var ConfigureResidentsPermissions = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ConfigureResidentsPermissions.ToString()
            };

            var DeleteAdministrator = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.DeleteAdministrator.ToString()
            };
            
            var ListNotifications = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ListNotifications.ToString()
            };

            var AddHomeOwnerRoleToUser = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.AddHomeOwnerRoleToUser.ToString()
            };

            
            var ChangeHardwareDeviceName = new SystemPermission
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ChangeHardwareDeviceName.ToString()
            };
            
            var CanReceiveNotifications = new HomePermission()
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.CanReceiveNotifications.ToString()
            };
            
            var BindDeviceToHomePermission = new HomePermission()
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.BindDeviceToHome.ToString()
            };
            
            var ListHomeDevicesPermission = new HomePermission()
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ListHomeDevices.ToString()
            };
            
            var ChangeHardwareDeviceNamePermission = new HomePermission()
            {
                Id = Guid.NewGuid().ToString(),
                Name = PermissionKey.ChangeHardwareDeviceName.ToString()
            };
            
            
            

            roleBuilder.HasData(
            new RoleSystemPermission
            (AdministratorRole.Id, CreateAdministrator.Id),
            new RoleSystemPermission
            (AdministratorRole.Id, DeleteAdministrator.Id),
            new RoleSystemPermission
            (AdministratorRole.Id, ListUsers.Id),
            new RoleSystemPermission
            (AdministratorRole.Id, ListCompanies.Id),
            new RoleSystemPermission
                (AdministratorRole.Id, CreateCompanyOwner.Id),
            new RoleSystemPermission
            (CompanyOwnerRole.Id, CreateCompany.Id),
            new RoleSystemPermission
            (CompanyOwnerRole.Id, RegisterSecurityCamera.Id),
            new RoleSystemPermission
            (CompanyOwnerRole.Id, RegisterWindowSensor.Id),
            new RoleSystemPermission
            (HomeOwnerRole.Id, ListDevices.Id),
            new RoleSystemPermission
            (HomeOwnerRole.Id, ListSupportedDevices.Id),
            new RoleSystemPermission
            (HomeOwnerRole.Id, CreateHome.Id),
            new RoleSystemPermission
            (HomeOwnerRole.Id, AddResidentToHome.Id),
            new RoleSystemPermission
            (HomeOwnerRole.Id, BindDeviceToHome.Id),
            new RoleSystemPermission
            (HomeOwnerRole.Id, ListHomeResidents.Id),
            new RoleSystemPermission
            (HomeOwnerRole.Id, ListHomeDevices.Id),
            new RoleSystemPermission
                (HomeOwnerRole.Id, ConfigureResidentsPermissions.Id),
            new RoleSystemPermission
                (HomeOwnerRole.Id, ListNotifications.Id),
            new RoleSystemPermission
                (AdministratorRole.Id, AddHomeOwnerRoleToUser.Id),
            new RoleSystemPermission
                (CompanyOwnerRole.Id, AddHomeOwnerRoleToUser.Id),
            new RoleSystemPermission
                (HomeOwnerRole.Id, ChangeHardwareDeviceName.Id)
            );
            

            modelBuilder.Entity<SystemPermission>()
                .HasData(
                CreateAdministrator,
                DeleteAdministrator,
                ListUsers,
                ListCompanies,
                CreateCompany,
                CreateCompanyOwner,
                RegisterSecurityCamera,
                RegisterWindowSensor,
                ListDevices,
                ListSupportedDevices,
                CreateHome,
                AddResidentToHome,
                BindDeviceToHome,
                ListHomeResidents,
                ListHomeDevices,
                ConfigureResidentsPermissions,
                ListNotifications,
                ChangeHardwareDeviceName,
                AddHomeOwnerRoleToUser
                );

            modelBuilder.Entity<HomePermission>().HasData(
                CanReceiveNotifications,
                BindDeviceToHomePermission,
                ListHomeDevicesPermission,
                ChangeHardwareDeviceNamePermission);
            

            modelBuilder.Entity<Role>().HasData(
                AdministratorRole,
                CompanyOwnerRole,
                HomeOwnerRole);

            var user = new User
            {
                Id = "diego-user-id",
                FirstName = "Diego",
                Email = "diegoaguirre1891@gmail.com",
                LastName = "Aguirre",
                FullName = "Diego Aguirre",
                Password = "$Aabbbdsddccdd1",
                CreationDate = DateTime.Now
            };

            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<UserRole>().HasData(new UserRole()
            {
                UserId = user.Id,
                RoleId = AdministratorRole.Id
            });
        }
    }

    public sealed record class RoleSystemPermission
    {
        public string RoleId { get; set; } = null!;
        public string SystemPermissionId { get; set; } = null!;
        public Role Role { get; set; } = null!;
        public SystemPermission Permission { get; set; } = null!;
        public RoleSystemPermission()
        {
        }

        public RoleSystemPermission(
            string roleId,
            string permissionId)
        {
            this.RoleId = roleId;
            this.SystemPermissionId = permissionId;
        }
    }
}