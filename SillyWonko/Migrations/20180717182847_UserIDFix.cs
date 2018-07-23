using Microsoft.EntityFrameworkCore.Migrations;

namespace SillyWonko.Migrations
{
    public partial class UserIDFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Carts",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Carts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
