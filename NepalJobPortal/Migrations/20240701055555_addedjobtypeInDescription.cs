using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NepalJobPortal.Migrations
{
    /// <inheritdoc />
    public partial class addedjobtypeInDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "JobDescription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobType",
                table: "JobDescription");
        }
    }
}
