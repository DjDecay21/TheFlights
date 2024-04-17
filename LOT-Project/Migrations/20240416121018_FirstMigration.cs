using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LOT_Project.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    departurePoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    arrivalPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AircraftType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
