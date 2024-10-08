using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapYourMeal.Migrations
{
    public partial class AddDishToReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dish",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dish",
                table: "Reviews");
        }
    }
}
