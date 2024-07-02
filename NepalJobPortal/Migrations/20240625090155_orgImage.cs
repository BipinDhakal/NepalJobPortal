using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NepalJobPortal.Migrations
{
    /// <inheritdoc />
    public partial class orgImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrgImage",
                table: "Organization",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgImage",
                table: "Organization");
        }
    }
}
