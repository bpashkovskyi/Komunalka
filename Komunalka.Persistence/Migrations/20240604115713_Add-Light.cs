using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Komunalka.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "komunalka");

            migrationBuilder.CreateTable(
                name: "Points",
                schema: "komunalka",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_AdditionalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordinates_OriginalLatitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_OriginalLongitude = table.Column<double>(type: "float", nullable: true),
                    Queue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Points",
                schema: "komunalka");
        }
    }
}
