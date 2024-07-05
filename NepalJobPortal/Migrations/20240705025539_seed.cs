using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NepalJobPortal.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "611b2ad5-6800-499a-b391-a35e39fd0d8a", "2", "Vendor", "VENDOR" },
                    { "955d43a6-c1ec-4d9d-addd-26c0bc93b90d", "1", "SuperAdmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OrgId", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "250b7a2c-c731-4b19-87d7-1059e721bd47", 0, "f0f2d968-900c-4f17-b15a-cb00bc82ee3e", "bipindhakal05@gmail.com", true, false, null, null, "BIPINDHAKAL05@GMAIL.COM", null, "AQAAAAIAAYagAAAAEJClfrD25VSql1DKeazBiBt5vGYicZTYq8XgBI3vhbtVWoXEV9WaZRIvWmP8O1Uw1A==", null, false, "fbc07dba-fa7d-41e3-b964-2786d34f9e35", false, "bipindhakal05@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "955d43a6-c1ec-4d9d-addd-26c0bc93b90d", "250b7a2c-c731-4b19-87d7-1059e721bd47" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "611b2ad5-6800-499a-b391-a35e39fd0d8a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "955d43a6-c1ec-4d9d-addd-26c0bc93b90d", "250b7a2c-c731-4b19-87d7-1059e721bd47" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "955d43a6-c1ec-4d9d-addd-26c0bc93b90d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "250b7a2c-c731-4b19-87d7-1059e721bd47");
        }
    }
}
