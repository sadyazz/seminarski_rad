﻿using eReservation.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace eReservation.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        public DbSet<Amenities> Amenities { get; set; }
        public DbSet<CalendarAvailability> CalendarAvailability { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CommentsOnReviews> CommentsOnReviews { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Images>Images { get; set; }
        public DbSet<PaymentMethods> PaymentMethods { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<PropertiesAmenities> PropertiesAmenities { get; set; }
        public DbSet<PropertyType>PropertyType { get; set;}
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Reviews>  Reviews { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertiesAmenities>()
                .HasKey(pa => new { pa.PropertiesID, pa.AmenitiesID });

            modelBuilder.Entity<PropertiesAmenities>()
                .HasOne(p => p.Properties)
                .WithMany(pa => pa.PropertiesAmenities)
                .HasForeignKey(p => p.PropertiesID);
            modelBuilder.Entity<PropertiesAmenities>()
              .HasOne(p => p.Amenities)
              .WithMany(pa => pa.PropertiesAmenities)
              .HasForeignKey(p => p.AmenitiesID);


            modelBuilder.Entity<Wishlist>()
                .HasKey(w => new { w.UserID, w.PropertiesID });
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(wl => wl.Wishlist)
                .HasForeignKey(w => w.UserID);
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Properties)
                .WithMany(wl => wl.Wishlist)
                .HasForeignKey(w => w.PropertiesID);
        }
    }
}
