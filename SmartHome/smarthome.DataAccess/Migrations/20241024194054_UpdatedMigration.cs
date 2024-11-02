using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace smarthome.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomePermissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPermissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleSystemPermission",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SystemPermissionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSystemPermission", x => new { x.RoleId, x.SystemPermissionId });
                    table.ForeignKey(
                        name: "FK_RoleSystemPermission_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleSystemPermission_SystemPermissions_SystemPermissionId",
                        column: x => x.SystemPermissionId,
                        principalTable: "SystemPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RUT = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_HouseNumber = table.Column<int>(type: "int", nullable: false),
                    Location_Latitude = table.Column<double>(type: "float", nullable: false),
                    Location_Longitude = table.Column<double>(type: "float", nullable: false),
                    QuantityOfResidents = table.Column<int>(type: "int", nullable: false),
                    QuantityOfResidentsAllowed = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homes_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HomeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomePermissionResident",
                columns: table => new
                {
                    HomePermissionsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResidentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePermissionResident", x => new { x.HomePermissionsId, x.ResidentId });
                    table.ForeignKey(
                        name: "FK_HomePermissionResident_HomePermissions_HomePermissionsId",
                        column: x => x.HomePermissionsId,
                        principalTable: "HomePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomePermissionResident_Memberships_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "Memberships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainPicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photographies = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotionDetectionEnabled = table.Column<bool>(type: "bit", nullable: true),
                    PersonDetectionEnabled = table.Column<bool>(type: "bit", nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devices_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HardwareDevices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HomeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HardwareDevices_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HardwareDevices_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HardwareDevices_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HardwareDeviceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDatetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_HardwareDevices_HardwareDeviceId",
                        column: x => x.HardwareDeviceId,
                        principalTable: "HardwareDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NotificationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasBeenRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HomePermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "56a5a463-c8b3-455d-a116-713ce73ff43d", "BindDeviceToHome" },
                    { "6ec80570-71b7-4a62-b54e-e637d082d65f", "ListHomeDevices" },
                    { "c34f262c-86fa-4e73-b484-0fd44e6b1327", "ChangeHardwareDeviceName" },
                    { "d48a4d1d-ef4c-4fa8-a2e8-be93e5979621", "DoesResidentCanReceiveNotifications" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "HomeOwner" },
                    { "51210374-e12c-4b29-b417-d8227e826929", "CompanyOwner" },
                    { "f5747f2a-2b33-4778-8a5d-2f23be7b5243", "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "SystemPermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "220831f1-3b1b-4628-bf8e-bc272e865a5e", "DeleteAdministrator" },
                    { "35d6faec-91f1-4720-9747-5a72502dbea9", "ListHomeResidents" },
                    { "36212d5d-1bc7-4266-b5ff-34f684f16c8f", "AddResidentToHome" },
                    { "52ab9e4c-ddad-4421-ae4a-04a58967b832", "CreateCompanyOwner" },
                    { "6e409d19-e48f-4add-8fad-dba65a2ba0cd", "AddHomeOwnerRoleToUser" },
                    { "73065a03-9f94-43d8-9193-93497fece87a", "RegisterWindowSensor" },
                    { "775f1b1f-e515-4bfb-88db-d27e9f6f09d0", "RegisterSecurityCamera" },
                    { "a1e6f910-325c-49aa-aafc-948ff8fdca1c", "ListUsers" },
                    { "a6deaf64-abb6-40b9-b8de-b7815f733ded", "ListSupportedDevices" },
                    { "b45ca2d3-4183-4d33-85fd-9c3552be7572", "CreateAdministrator" },
                    { "bc942ea0-1a6f-4bf9-9427-9ab50bee6ad6", "BindDeviceToHome" },
                    { "c12b0156-7d89-4139-bd59-6a9972c7fcb6", "ChangeHardwareDeviceName" },
                    { "c51e2d8d-85aa-4c49-8b39-7e882975f871", "ListNotifications" },
                    { "cb3abca3-8d95-4ce3-bf3a-b923c4b47b87", "ConfigureResidentsPermissions" },
                    { "cf45ebc8-35f4-4fd4-ad15-aa9b9d99e718", "ListCompanies" },
                    { "db2be08c-6e84-4527-88e3-45e7b64b42b0", "CreateHome" },
                    { "f5fd16a4-1015-4f98-a281-666b3abcccb8", "ListDevices" },
                    { "f6e40e09-f30a-4d46-afa7-21710f34805f", "CreateCompany" },
                    { "fcbb1fba-bcce-474e-b088-393f1b40e47c", "ListHomeDevices" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "FirstName", "FullName", "IsComplete", "LastName", "Password", "ProfilePicture" },
                values: new object[] { "diego-user-id", new DateTime(2024, 10, 24, 16, 40, 54, 190, DateTimeKind.Local).AddTicks(969), "diegoaguirre1891@gmail.com", "Diego", "Diego Aguirre", null, "Aguirre", "$Aabbbdsddccdd1", null });

            migrationBuilder.InsertData(
                table: "RoleSystemPermission",
                columns: new[] { "RoleId", "SystemPermissionId" },
                values: new object[,]
                {
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "35d6faec-91f1-4720-9747-5a72502dbea9" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "36212d5d-1bc7-4266-b5ff-34f684f16c8f" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "a6deaf64-abb6-40b9-b8de-b7815f733ded" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "bc942ea0-1a6f-4bf9-9427-9ab50bee6ad6" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "c12b0156-7d89-4139-bd59-6a9972c7fcb6" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "c51e2d8d-85aa-4c49-8b39-7e882975f871" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "cb3abca3-8d95-4ce3-bf3a-b923c4b47b87" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "db2be08c-6e84-4527-88e3-45e7b64b42b0" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "f5fd16a4-1015-4f98-a281-666b3abcccb8" },
                    { "3e31642f-9719-4c02-9c59-0b1459eb52a2", "fcbb1fba-bcce-474e-b088-393f1b40e47c" },
                    { "51210374-e12c-4b29-b417-d8227e826929", "6e409d19-e48f-4add-8fad-dba65a2ba0cd" },
                    { "51210374-e12c-4b29-b417-d8227e826929", "73065a03-9f94-43d8-9193-93497fece87a" },
                    { "51210374-e12c-4b29-b417-d8227e826929", "775f1b1f-e515-4bfb-88db-d27e9f6f09d0" },
                    { "51210374-e12c-4b29-b417-d8227e826929", "f6e40e09-f30a-4d46-afa7-21710f34805f" },
                    { "f5747f2a-2b33-4778-8a5d-2f23be7b5243", "220831f1-3b1b-4628-bf8e-bc272e865a5e" },
                    { "f5747f2a-2b33-4778-8a5d-2f23be7b5243", "52ab9e4c-ddad-4421-ae4a-04a58967b832" },
                    { "f5747f2a-2b33-4778-8a5d-2f23be7b5243", "6e409d19-e48f-4add-8fad-dba65a2ba0cd" },
                    { "f5747f2a-2b33-4778-8a5d-2f23be7b5243", "a1e6f910-325c-49aa-aafc-948ff8fdca1c" },
                    { "f5747f2a-2b33-4778-8a5d-2f23be7b5243", "b45ca2d3-4183-4d33-85fd-9c3552be7572" },
                    { "f5747f2a-2b33-4778-8a5d-2f23be7b5243", "cf45ebc8-35f4-4fd4-ad15-aa9b9d99e718" }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f5747f2a-2b33-4778-8a5d-2f23be7b5243", "diego-user-id" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_OwnerId",
                table: "Companies",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_RoomId",
                table: "Devices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareDevices_DeviceId",
                table: "HardwareDevices",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareDevices_HomeId",
                table: "HardwareDevices",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareDevices_RoomId",
                table: "HardwareDevices",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HomePermissionResident_ResidentId",
                table: "HomePermissionResident",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_OwnerId",
                table: "Homes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_HomeId",
                table: "Memberships",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_HardwareDeviceId",
                table: "Notifications",
                column: "HardwareDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleSystemPermission_SystemPermissionId",
                table: "RoleSystemPermission",
                column: "SystemPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_HomeId",
                table: "Room",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomePermissionResident");

            migrationBuilder.DropTable(
                name: "RoleSystemPermission");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "HomePermissions");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "SystemPermissions");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "HardwareDevices");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
