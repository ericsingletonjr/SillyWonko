using Microsoft.EntityFrameworkCore.Migrations;

namespace SillyWonko.Migrations
{
    public partial class addproductid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductID",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "CartItems",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductID",
                table: "CartItems",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductID",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "CartItems",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductID",
                table: "CartItems",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
