using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppIdentity.Migrations
{
    public partial class AddPriceToRecipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Recipes",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "CreatedById",
                value: "f74e8757-8e4a-442e-9742-f6d6ac214a81");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Recipes");

            migrationBuilder.UpdateData(
                table: "Recipes",
                keyColumn: "RecipeId",
                keyValue: 1,
                column: "CreatedById",
                value: "d6033247-6924-4338-813e-46e111949e64");
        }
    }
}
