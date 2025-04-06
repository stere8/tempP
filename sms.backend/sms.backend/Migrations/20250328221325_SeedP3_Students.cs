using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sms.backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedP3_Students : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeLevel",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradeLevel",
                table: "Lessons");
        }
    }
}
