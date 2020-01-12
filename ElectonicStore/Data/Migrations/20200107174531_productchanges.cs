using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectonicStore.Data.Migrations
{
    public partial class productchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "image1",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "image2",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "image3",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "image4",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "Product",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image1",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image2",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image3",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image4",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
