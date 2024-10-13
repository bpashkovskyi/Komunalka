using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Komunalka.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_Region",
                schema: "komunalka",
                table: "Points",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Region",
                schema: "komunalka",
                table: "Points");
        }
    }
}
