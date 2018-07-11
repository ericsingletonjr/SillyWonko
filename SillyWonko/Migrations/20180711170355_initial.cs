using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SillyWonko.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Sku = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Description", "Image", "Name", "Price", "Sku" },
                values: new object[,]
                {
                    { 1, "description", "", "product1", 9.99m, "ABCD0001" },
                    { 2, "description", "", "product2", 9.99m, "ABCD0001" },
                    { 3, "description", "", "product3", 9.99m, "ABCD0001" },
                    { 4, "description", "", "product4", 9.99m, "ABCD0001" },
                    { 5, "description", "", "product5", 9.99m, "ABCD0001" },
                    { 6, "description", "", "product6", 9.99m, "ABCD0001" },
                    { 7, "description", "", "product7", 9.99m, "ABCD0001" },
                    { 8, "description", "", "product8", 9.99m, "ABCD0001" },
                    { 9, "description", "", "product9", 9.99m, "ABCD0001" },
                    { 10, "description", "", "product10", 9.99m, "ABCD0001" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
