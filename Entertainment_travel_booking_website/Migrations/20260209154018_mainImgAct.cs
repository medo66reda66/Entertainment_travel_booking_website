using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entertainment_travel_booking_website.Migrations
{
    /// <inheritdoc />
    public partial class mainImgAct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trips_additianActivites_AdditionalActivityId",
                table: "trips");

            migrationBuilder.DropIndex(
                name: "IX_trips_AdditionalActivityId",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "AdditionalActivityId",
                table: "trips");

            migrationBuilder.AddColumn<string>(
                name: "MainImg",
                table: "additianActivites",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainImg",
                table: "additianActivites");

            migrationBuilder.AddColumn<int>(
                name: "AdditionalActivityId",
                table: "trips",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_trips_AdditionalActivityId",
                table: "trips",
                column: "AdditionalActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_trips_additianActivites_AdditionalActivityId",
                table: "trips",
                column: "AdditionalActivityId",
                principalTable: "additianActivites",
                principalColumn: "Id");
        }
    }
}
