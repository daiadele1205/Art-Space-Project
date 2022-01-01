using Microsoft.EntityFrameworkCore.Migrations;

namespace Art.Migrations
{
    public partial class removedSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ArtworkPortfolio");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ArtworkPortfolio",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
