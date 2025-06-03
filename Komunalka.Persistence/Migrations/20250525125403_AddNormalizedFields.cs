using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Komunalka.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedDistributorName",
                schema: "komunalka",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedType",
                schema: "komunalka",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedDistributorName",
                schema: "komunalka",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "NormalizedType",
                schema: "komunalka",
                table: "Boards");
        }
    }
}
