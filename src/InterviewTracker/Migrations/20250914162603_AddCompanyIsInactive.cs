using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyIsInactive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInactive",
                table: "Companies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInactive",
                table: "Companies");
        }
    }
}
