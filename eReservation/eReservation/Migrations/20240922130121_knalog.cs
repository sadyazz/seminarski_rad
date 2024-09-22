using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eReservation.Migrations
{
    /// <inheritdoc />
    public partial class knalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is2FOtkljucano",
                table: "KorisnickiNalog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is2FOtkljucano",
                table: "KorisnickiNalog",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
