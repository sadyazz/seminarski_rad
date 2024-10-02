﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eReservation.Data;

#nullable disable

namespace eReservation.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240902190149_removeWishlistTest")]
    partial class removeWishlistTest
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("eReservation.Models.Amenities", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Amenities");
                });

            modelBuilder.Entity("eReservation.Models.AutentifikacijaToken", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("Is2FOtkljucano")
                        .HasColumnType("bit");

                    b.Property<int>("KorisnickiNalogId")
                        .HasColumnType("int");

                    b.Property<string>("TwoFKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ipAdresa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vrijednost")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("vrijemeEvidentiranja")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("KorisnickiNalogId");

                    b.ToTable("AutentifikacijaToken");
                });

            modelBuilder.Entity("eReservation.Models.CalendarAvailability", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Availability")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("PropertiesID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("PropertiesID");

                    b.ToTable("CalendarAvailability");
                });

            modelBuilder.Entity("eReservation.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CountryID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CountryID");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("eReservation.Models.CommentsOnReviews", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReviewID")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ReviewID");

                    b.HasIndex("UserID");

                    b.ToTable("CommentsOnReviews");
                });

            modelBuilder.Entity("eReservation.Models.Country", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("eReservation.Models.Images", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PropertyId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("PropertyId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("eReservation.Models.KorisnickiNalog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<bool>("Is2FActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("KorisnickiNalog");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("eReservation.Models.LogKretanjePoSistemu", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("ExceptionMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsException")
                        .HasColumnType("bit");

                    b.Property<string>("PostData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QueryPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("LogKretanjePoSistemu");
                });

            modelBuilder.Entity("eReservation.Models.PaymentMethods", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("eReservation.Models.Properties", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfBathrooms")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfRooms")
                        .HasColumnType("int");

                    b.Property<int>("PricePerNight")
                        .HasColumnType("int");

                    b.Property<int>("PropertyTypeID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.HasIndex("PropertyTypeID");

                    b.HasIndex("UserID");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("eReservation.Models.PropertiesAmenities", b =>
                {
                    b.Property<int>("PropertiesID")
                        .HasColumnType("int");

                    b.Property<int>("AmenitiesID")
                        .HasColumnType("int");

                    b.HasKey("PropertiesID", "AmenitiesID");

                    b.HasIndex("AmenitiesID");

                    b.ToTable("PropertiesAmenities");
                });

            modelBuilder.Entity("eReservation.Models.PropertyType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("PropertyType");
                });

            modelBuilder.Entity("eReservation.Models.Reservations", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateOfArrival")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfDeparture")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentMethodsID")
                        .HasColumnType("int");

                    b.Property<int>("PropertiesID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("PaymentMethodsID");

                    b.HasIndex("PropertiesID");

                    b.HasIndex("UserID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("eReservation.Models.Reviews", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateReview")
                        .HasColumnType("datetime2");

                    b.Property<int>("PropertiesID")
                        .HasColumnType("int");

                    b.Property<int>("Review")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("PropertiesID");

                    b.HasIndex("UserID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("eReservation.Models.Admin", b =>
                {
                    b.HasBaseType("eReservation.Models.KorisnickiNalog");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("eReservation.Models.User", b =>
                {
                    b.HasBaseType("eReservation.Models.KorisnickiNalog");

                    b.Property<DateTime>("DateBirth")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfRegistraion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("User");
                });

            modelBuilder.Entity("eReservation.Models.AutentifikacijaToken", b =>
                {
                    b.HasOne("eReservation.Models.KorisnickiNalog", "korisnickiNalog")
                        .WithMany()
                        .HasForeignKey("KorisnickiNalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("korisnickiNalog");
                });

            modelBuilder.Entity("eReservation.Models.CalendarAvailability", b =>
                {
                    b.HasOne("eReservation.Models.Properties", "Properties")
                        .WithMany()
                        .HasForeignKey("PropertiesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Properties");
                });

            modelBuilder.Entity("eReservation.Models.City", b =>
                {
                    b.HasOne("eReservation.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("eReservation.Models.CommentsOnReviews", b =>
                {
                    b.HasOne("eReservation.Models.Reviews", "Reviews")
                        .WithMany()
                        .HasForeignKey("ReviewID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eReservation.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reviews");

                    b.Navigation("User");
                });

            modelBuilder.Entity("eReservation.Models.Images", b =>
                {
                    b.HasOne("eReservation.Models.Properties", "Property")
                        .WithMany("Images")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");
                });

            modelBuilder.Entity("eReservation.Models.LogKretanjePoSistemu", b =>
                {
                    b.HasOne("eReservation.Models.KorisnickiNalog", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("eReservation.Models.Properties", b =>
                {
                    b.HasOne("eReservation.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eReservation.Models.PropertyType", "PropertyType")
                        .WithMany()
                        .HasForeignKey("PropertyTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eReservation.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("PropertyType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("eReservation.Models.PropertiesAmenities", b =>
                {
                    b.HasOne("eReservation.Models.Amenities", "Amenities")
                        .WithMany("PropertiesAmenities")
                        .HasForeignKey("AmenitiesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eReservation.Models.Properties", "Properties")
                        .WithMany("PropertiesAmenities")
                        .HasForeignKey("PropertiesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amenities");

                    b.Navigation("Properties");
                });

            modelBuilder.Entity("eReservation.Models.Reservations", b =>
                {
                    b.HasOne("eReservation.Models.PaymentMethods", "PaymentMethods")
                        .WithMany()
                        .HasForeignKey("PaymentMethodsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eReservation.Models.Properties", "Properties")
                        .WithMany()
                        .HasForeignKey("PropertiesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eReservation.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentMethods");

                    b.Navigation("Properties");

                    b.Navigation("User");
                });

            modelBuilder.Entity("eReservation.Models.Reviews", b =>
                {
                    b.HasOne("eReservation.Models.Properties", "Properties")
                        .WithMany("Reviews")
                        .HasForeignKey("PropertiesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eReservation.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Properties");

                    b.Navigation("User");
                });

            modelBuilder.Entity("eReservation.Models.Admin", b =>
                {
                    b.HasOne("eReservation.Models.KorisnickiNalog", null)
                        .WithOne()
                        .HasForeignKey("eReservation.Models.Admin", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("eReservation.Models.User", b =>
                {
                    b.HasOne("eReservation.Models.KorisnickiNalog", null)
                        .WithOne()
                        .HasForeignKey("eReservation.Models.User", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("eReservation.Models.Amenities", b =>
                {
                    b.Navigation("PropertiesAmenities");
                });

            modelBuilder.Entity("eReservation.Models.Properties", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("PropertiesAmenities");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
