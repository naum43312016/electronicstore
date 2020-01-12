using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectonicStore.Data.Migrations
{
    public partial class AddOrderemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OrderDetails",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "OrderDetails");
        }
    }
}
