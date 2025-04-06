using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sms.backend.Migrations
{
    /// <inheritdoc />
    public partial class teachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "Email", "FirstName", "LastName", "SubjectExpertise" },
                values: new object[,]
                {
                    { 1, "alice.johnson@mail.com", "Alice", "Johnson", "Mathematics" },
                    { 2, "bob.williams@mail.com", "Bob", "Williams", "Science" },
                    { 3, "charlie.linguist@mail.com", "Charlie", "Linguist", "Kinyarwanda" },
                    { 4, "diana.english@mail.com", "Diana", "English", "English" },
                    { 5, "edward.historian@mail.com", "Edward", "Historian", "Social Studies" },
                    { 6, "fiona.coach@mail.com", "Fiona", "Coach", "Physical Education" },
                    { 7, "grace.matheson@mail.com", "Grace", "Matheson", "Mathematics" },
                    { 8, "henry.scientist@mail.com", "Henry", "Scientist", "Science" },
                    { 9, "isabel.linguist@mail.com", "Isabel", "Linguist", "Kinyarwanda" },
                    { 10, "jack.english@mail.com", "Jack", "English", "English" },
                    { 11, "karen.historian@mail.com", "Karen", "Historian", "Social Studies" },
                    { 12, "leo.coach@mail.com", "Leo", "Coach", "Physical Education" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 12);
        }
    }
}
