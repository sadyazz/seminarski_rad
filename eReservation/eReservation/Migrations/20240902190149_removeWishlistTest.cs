using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eReservation.Migrations
{
    /// <inheritdoc />
    public partial class removeWishlistTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wishlist");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PropertiesID = table.Column<int>(type: "int", nullable: false),
                    DateAdd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => new { x.UserID, x.PropertiesID });
                    table.ForeignKey(
                        name: "FK_Wishlist_Properties_PropertiesID",
                        column: x => x.PropertiesID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlist_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_PropertiesID",
                table: "Wishlist",
                column: "PropertiesID");
        }
    }
}
