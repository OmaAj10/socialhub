using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Social_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Address", "BirthDate", "City", "CreatedBy", "Date", "Email", "Title" },
                values: new object[,]
                {
                    { 1, "B-Vallen", new DateTime(1991, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bollebygd", "Omar", new DateTime(2024, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar@example.com", "Spela Fotboll" },
                    { 2, "Hulta", new DateTime(1992, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bollebygd", "Fighter", new DateTime(2024, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fighter@example.com", "Spela Golf" },
                    { 3, "Bollebygd skolan", new DateTime(1993, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bollebygd", "Bob", new DateTime(2024, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bob@example.com", "Basket Match" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
