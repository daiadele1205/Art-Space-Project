using Microsoft.EntityFrameworkCore.Migrations;

namespace Art.Migrations
{
    public partial class deleteImageColumnFromArtworkType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ArtworkType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ArtworkType",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
