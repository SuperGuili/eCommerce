using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceWebApp.Migrations
{
    public partial class AddedFieldsToProductAndTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TagDiscountPCent",
                table: "Tags",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TagId2",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TagId3",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagDiscountPCent",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TagId2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TagId3",
                table: "Products");
        }
    }
}
