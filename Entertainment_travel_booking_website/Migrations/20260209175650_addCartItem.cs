using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class addCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartItemId",
                table: "additianActivites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cartItems_trips_TripId",
                        column: x => x.TripId,
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_additianActivites_CartItemId",
                table: "additianActivites",
                column: "CartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_TripId",
                table: "cartItems",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_additianActivites_cartItems_CartItemId",
                table: "additianActivites",
                column: "CartItemId",
                principalTable: "cartItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_additianActivites_cartItems_CartItemId",
                table: "additianActivites");

            migrationBuilder.DropTable(
                name: "cartItems");

            migrationBuilder.DropIndex(
                name: "IX_additianActivites_CartItemId",
                table: "additianActivites");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "additianActivites");
        }
    }
}
