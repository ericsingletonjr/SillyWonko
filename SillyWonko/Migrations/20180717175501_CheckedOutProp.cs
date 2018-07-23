using Microsoft.EntityFrameworkCore.Migrations;

namespace SillyWonko.Migrations
{
    public partial class CheckedOutProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedOut",
                table: "Carts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCheckedOut",
                table: "Carts");
        }
    }
}
