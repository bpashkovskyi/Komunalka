using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Komunalka.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reasons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_AdditionalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment_SurfaceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment_SurfaceCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment_Lighting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment_RoadElements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment_Constructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment_Weather = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment_TrafficTools = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Environment_IsPlaceOfAccidentConcentration = table.Column<bool>(type: "bit", nullable: true),
                    Environment_NotParsed = table.Column<bool>(type: "bit", nullable: true),
                    Casualties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordinates_OriginalLatitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_OriginalLongitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_ShiftedLatitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_ShiftedLongitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_ManuallyCorrectedLatitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_ManuallyCorrectedLongitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accidents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Protocols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Heard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Decided = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProtocolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Protocols_ProtocolId",
                        column: x => x.ProtocolId,
                        principalTable: "Protocols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProtocolId",
                table: "Items",
                column: "ProtocolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accidents");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Protocols");
        }
    }
}
