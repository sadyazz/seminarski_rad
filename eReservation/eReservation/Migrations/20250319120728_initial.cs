using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eReservation.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KorisnickiNalog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is2FActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnickiNalog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Admin_KorisnickiNalog_ID",
                        column: x => x.ID,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutentifikacijaToken",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vrijednost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KorisnickiNalogId = table.Column<int>(type: "int", nullable: false),
                    vrijemeEvidentiranja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ipAdresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Is2FOtkljucano = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutentifikacijaToken", x => x.id);
                    table.ForeignKey(
                        name: "FK_AutentifikacijaToken_KorisnickiNalog_KorisnickiNalogId",
                        column: x => x.KorisnickiNalogId,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogKretanjePoSistemu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    QueryPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsException = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogKretanjePoSistemu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogKretanjePoSistemu_KorisnickiNalog_UserID",
                        column: x => x.UserID,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfRegistraion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_KorisnickiNalog_ID",
                        column: x => x.ID,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfRooms = table.Column<int>(type: "int", nullable: false),
                    NumberOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    PricePerNight = table.Column<int>(type: "int", nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    PropertyTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Properties_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_PropertyType_PropertyTypeID",
                        column: x => x.PropertyTypeID,
                        principalTable: "PropertyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarAvailability",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertiesID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Availability = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarAvailability", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CalendarAvailability_Properties_PropertiesID",
                        column: x => x.PropertiesID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Images_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertiesAmenities",
                columns: table => new
                {
                    PropertiesID = table.Column<int>(type: "int", nullable: false),
                    AmenitiesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesAmenities", x => new { x.PropertiesID, x.AmenitiesID });
                    table.ForeignKey(
                        name: "FK_PropertiesAmenities_Amenities_AmenitiesID",
                        column: x => x.AmenitiesID,
                        principalTable: "Amenities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertiesAmenities_Properties_PropertiesID",
                        column: x => x.PropertiesID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PropertiesID = table.Column<int>(type: "int", nullable: false),
                    DateOfArrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfDeparture = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethodsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reservations_PaymentMethods_PaymentMethodsID",
                        column: x => x.PaymentMethodsID,
                        principalTable: "PaymentMethods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reservations_Properties_PropertiesID",
                        column: x => x.PropertiesID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reservations_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PropertiesID = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateReview = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reviews_Properties_PropertiesID",
                        column: x => x.PropertiesID,
                        principalTable: "Properties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reviews_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CommentsOnReviews",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsOnReviews", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CommentsOnReviews_Reviews_ReviewID",
                        column: x => x.ReviewID,
                        principalTable: "Reviews",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentsOnReviews_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutentifikacijaToken_KorisnickiNalogId",
                table: "AutentifikacijaToken",
                column: "KorisnickiNalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarAvailability_PropertiesID",
                table: "CalendarAvailability",
                column: "PropertiesID");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryID",
                table: "Cities",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsOnReviews_ReviewID",
                table: "CommentsOnReviews",
                column: "ReviewID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsOnReviews_UserID",
                table: "CommentsOnReviews",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_PropertyId",
                table: "Images",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_LogKretanjePoSistemu_UserID",
                table: "LogKretanjePoSistemu",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CityID",
                table: "Properties",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyTypeID",
                table: "Properties",
                column: "PropertyTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UserID",
                table: "Properties",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PropertiesAmenities_AmenitiesID",
                table: "PropertiesAmenities",
                column: "AmenitiesID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PaymentMethodsID",
                table: "Reservations",
                column: "PaymentMethodsID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PropertiesID",
                table: "Reservations",
                column: "PropertiesID");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserID",
                table: "Reservations",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PropertiesID",
                table: "Reviews",
                column: "PropertiesID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "AutentifikacijaToken");

            migrationBuilder.DropTable(
                name: "CalendarAvailability");

            migrationBuilder.DropTable(
                name: "CommentsOnReviews");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "LogKretanjePoSistemu");

            migrationBuilder.DropTable(
                name: "PropertiesAmenities");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "PropertyType");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "KorisnickiNalog");
        }
    }
}
