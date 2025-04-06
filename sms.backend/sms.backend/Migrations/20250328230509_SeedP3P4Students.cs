using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sms.backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedP3P4Students : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2015, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Léon", "Mugenzi" },
                    { 2, new DateTime(2015, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marie", "Mukamana" },
                    { 3, new DateTime(2015, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Henri", "Ndayisenga" },
                    { 4, new DateTime(2015, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Claire", "Mukarutabana" },
                    { 5, new DateTime(2015, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pierre", "Kamanzi" },
                    { 6, new DateTime(2015, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Isabelle", "Ingabire" },
                    { 7, new DateTime(2015, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Michel", "Munyaneza" },
                    { 8, new DateTime(2015, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alice", "Murekatete" },
                    { 9, new DateTime(2015, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Georges", "Uwimana" },
                    { 10, new DateTime(2015, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Catherine", "Mukasine" },
                    { 11, new DateTime(2015, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Denis", "Habimana" },
                    { 12, new DateTime(2015, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brigitte", "Mukamugisha" },
                    { 13, new DateTime(2015, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "André", "Uwimbabazi" },
                    { 14, new DateTime(2015, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christine", "Murema" },
                    { 15, new DateTime(2015, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bernard", "Kamanzi" },
                    { 16, new DateTime(2015, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jeanne", "Mukashyaka" },
                    { 17, new DateTime(2014, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Louis", "Mukankuranga" },
                    { 18, new DateTime(2014, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lola", "Rukundo" },
                    { 19, new DateTime(2014, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zoe", "Murema" },
                    { 20, new DateTime(2014, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anna", "Karekezi" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentId",
                keyValue: 20);
        }
    }
}
