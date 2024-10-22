using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MapYourMeal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRestaurantModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Restaurants",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Restaurants",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Restaurants",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Restaurants",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Restaurants",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Restaurants");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Restaurants",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Restaurants",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}
