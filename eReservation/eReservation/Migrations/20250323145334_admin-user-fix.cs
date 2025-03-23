using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eReservation.Migrations
{
    /// <inheritdoc />
    public partial class adminuserfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAdmin",
                table: "KorisnickiNalog",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isUser",
                table: "KorisnickiNalog",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "KorisnickiNalog");

            migrationBuilder.DropColumn(
                name: "isUser",
                table: "KorisnickiNalog");
        }
    }
}
