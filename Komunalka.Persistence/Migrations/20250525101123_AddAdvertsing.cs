using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Komunalka.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvertsing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                schema: "komunalka",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issued = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorityIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistributorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistributorIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidThrough = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractDateSigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanesValue = table.Column<double>(type: "float", nullable: true),
                    HorizontalSizeValue = table.Column<double>(type: "float", nullable: true),
                    VerticalSizeValue = table.Column<double>(type: "float", nullable: true),
                    SquareValue = table.Column<double>(type: "float", nullable: true),
                    Address_PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_AdminUnitL1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_AdminUnitL2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_AdminUnitL3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_PostName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Thoroughfare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_LocatorDesignator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_LocatorBuilding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordinates_OriginalLatitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_OriginalLongitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_ShiftedLatitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_ShiftedLongitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_ManuallyCorrectedLatitude = table.Column<double>(type: "float", nullable: true),
                    Coordinates_ManuallyCorrectedLongitude = table.Column<double>(type: "float", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boards",
                schema: "komunalka");
        }
    }
}
