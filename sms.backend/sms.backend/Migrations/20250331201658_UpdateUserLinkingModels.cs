using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sms.backend.Migrations
{
    public partial class UpdateUserLinkingModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add UserId column to Students table.
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            // Add UserId column to Staff table.
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Staff",
                type: "nvarchar(450)",
                nullable: true);

            // Drop the foreign key from StudentParents to AspNetUsers on ParentId.
            migrationBuilder.DropForeignKey(
                name: "FK_StudentParents_AspNetUsers_ParentId",
                table: "StudentParents");

            // Drop the primary key constraint on StudentParents.
            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentParents",
                table: "StudentParents");

            // Alter the ParentId column from int to string (nvarchar(450)).
            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                table: "StudentParents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Recreate the primary key with the new column type.
            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentParents",
                table: "StudentParents",
                columns: new[] { "StudentId", "ParentId" });

            // Create the Parents table.
            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    ParentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.ParentId);
                    table.ForeignKey(
                        name: "FK_Parents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            // Create indexes for new columns.
            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserId",
                table: "Staff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_UserId",
                table: "Parents",
                column: "UserId");

            // Add foreign keys linking Students and Staff to AspNetUsers.
            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_AspNetUsers_UserId",
                table: "Staff",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            // Recreate the foreign key for StudentParents to reference AspNetUsers with the new column type.
            migrationBuilder.AddForeignKey(
                name: "FK_StudentParents_AspNetUsers_ParentId",
                table: "StudentParents",
                column: "ParentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_AspNetUsers_UserId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentParents_AspNetUsers_ParentId",
                table: "StudentParents");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Staff_UserId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Staff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentParents",
                table: "StudentParents");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                table: "StudentParents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentParents",
                table: "StudentParents",
                columns: new[] { "StudentId", "ParentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentParents_AspNetUsers_ParentId",
                table: "StudentParents",
                column: "ParentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
