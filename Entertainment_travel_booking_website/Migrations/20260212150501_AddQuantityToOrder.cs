using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Orders");
        }
    }
}
