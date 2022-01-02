using Microsoft.EntityFrameworkCore.Migrations;

namespace Art.Migrations
{
    public partial class addImageToArtworkType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ArtworkType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ArtworkType");
        }
    }
}
