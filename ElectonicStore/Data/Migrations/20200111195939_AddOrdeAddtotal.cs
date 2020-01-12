using Microsoft.EntityFrameworkCore.Migrations;

namespace ElectonicStore.Data.Migrations
{
    public partial class AddOrdeAddtotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "total",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total",
                table: "OrderDetails");
        }
    }
}
