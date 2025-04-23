using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyDAL.Models;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyDAL.Contexts
{
    public class PropertyDbContext : IdentityDbContext<User>
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Amenity
            // Primary Key
            builder.Entity<Amenity>().HasKey(a => a.Id);

            // Properities
            builder.Entity<Amenity>().Property(a => a.Name).HasMaxLength(100);
            builder.Entity<Amenity>().Property(a => a.IconClass).HasMaxLength(100);
            #endregion

            #region AmenityCategory
            // Primary Key
            builder.Entity<AmenityCategory>().HasKey(ac => ac.Id);

            // Properities
            builder.Entity<AmenityCategory>().Property(ac => ac.Name).HasMaxLength(100);

            // Relationship With Amenitis
            builder.Entity<AmenityCategory>()
                .HasMany(ac => ac.Amenities)
                .WithOne(a => a.AmenityCategory)
                .HasForeignKey(a => a.AmenityCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Booking
            // Composite Primary Key
            builder.Entity<Booking>().HasKey(b => b.Id);

            // Properties 
            builder.Entity<Booking>().Property(b => b.StartDate).HasColumnType("date");
            builder.Entity<Booking>().Property(b => b.EndDate).HasColumnType("date");
            builder.Entity<Booking>().Property(b => b.FeePerMonth).HasColumnType("decimal(12,2)");
            builder.Entity<Booking>().Property(b => b.IsActive).HasDefaultValue(true);

            // Relationships With User 
            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.BookingsUser)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Host)
                .WithMany(u => u.BookingsHost)
                .HasForeignKey(b => b.HostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships With Property
            builder.Entity<Booking>()
                .HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships With Rating
            builder.Entity<Booking>().
                HasOne(b => b.Rating)
                .WithOne(r => r.Booking)
                .HasForeignKey<Rating>(r => r.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Country
            // Primary Key
            builder.Entity<Country>().HasKey(c => c.Id);

            // Properties 
            builder.Entity<Country>().Property(p => p.Name).HasMaxLength(100);
            builder.Entity<Country>().Property(p => p.Code).HasMaxLength(100);
            builder.Entity<Country>().Property(p => p.latitude).HasColumnType("decimal(9,6)");
            builder.Entity<Country>().Property(p => p.longitude).HasColumnType("decimal(9,6)");

            #endregion

            #region Favourite
            builder.Entity<Favourite>().HasKey(f => new { f.UserId, f.PropertyId });

            // Relationship with user
            builder.Entity<Favourite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favourites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with property
            builder.Entity<Favourite>()
                .HasOne(f => f.Property)
                .WithMany(p => p.Favourites)
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Image
            // Primary Key
            builder.Entity<Image>().HasKey(i => i.Id);

            // Properties 
            builder.Entity<Image>().Property(i => i.Path).HasMaxLength(255);
            builder.Entity<Image>().Property(i => i.IsPrimary).HasDefaultValue(false);

            // Relationship With Property 
            builder.Entity<Image>()
                .HasOne(i => i.Property)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Location
            // Primary Key
            builder.Entity<Location>().HasKey(l => l.Id);

            // Properties 
            builder.Entity<Location>().Property(l => l.Country).HasMaxLength(100);
            builder.Entity<Location>().Property(l => l.City).HasMaxLength(100);
            builder.Entity<Location>().Property(l => l.State).HasMaxLength(100);
            builder.Entity<Location>().Property(l => l.Latitude).HasColumnType("decimal(9,6)");
            builder.Entity<Location>().Property(l => l.Longitude).HasColumnType("decimal(9,6)");

            // Relationship With Property
            builder.Entity<Location>()
                    .HasOne(l => l.Property)
                    .WithOne(p => p.Location)
                    .HasForeignKey<Property>(p => p.LocationId)
                    .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Notification
            // Primary Key
            builder.Entity<Notification>().HasKey(n => n.Id);

            // Properties 
            builder.Entity<Notification>().Property(n => n.Title).HasMaxLength(100);
            builder.Entity<Notification>().Property(n => n.Content).HasMaxLength(2000);
            builder.Entity<Notification>().Property(n => n.CreatedAt).HasColumnType("date").HasDefaultValueSql("GETDATE()");

            #endregion

            #region Property
            // Primary Key
            builder.Entity<Property>().HasKey(p => p.Id);

            // Properties 
            builder.Entity<Property>().Property(p => p.Name).HasMaxLength(100);
            builder.Entity<Property>().Property(p => p.Description).HasMaxLength(5000);
            builder.Entity<Property>().Property(p => p.Address).HasMaxLength(200);
            builder.Entity<Property>().Property(p => p.IsListed).IsRequired().HasDefaultValue(false);
            builder.Entity<Property>().Property(p => p.IsFeatured).IsRequired().HasDefaultValue(false);
            builder.Entity<Property>().Property(p => p.ListedAt).HasColumnType("date");
            builder.Entity<Property>().Property(p => p.UnListDate).HasColumnType("date");
            builder.Entity<Property>().Property(p => p.ListingType).HasConversion<string>().HasMaxLength(20);


            #endregion

            #region PropertyAmenity
            builder.Entity<PropertyAmenity>().HasKey(pa => new { pa.PropertyId, pa.AmenityId });

            // Relationship with property
            builder.Entity<PropertyAmenity>()
                .HasOne(pa => pa.Property)
                .WithMany(p => p.Amenities)
                .HasForeignKey(pa => pa.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with amenity
            builder.Entity<PropertyAmenity>()
                .HasOne(pa => pa.Amenity)
                .WithMany(a => a.Properties)
                .HasForeignKey(pa => pa.AmenityId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region PropertyService
            //builder.Entity<PropertyService>().HasKey(pa => new { pa.PropertyId, pa.ServiceId });

            //// Relationship with property
            //builder.Entity<PropertyService>()
            //    .HasOne(ps => ps.Property)
            //    .WithMany(p => p.Services)
            //    .HasForeignKey(ps => ps.PropertyId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// Relationship with service
            //builder.Entity<PropertyService>()
            //    .HasOne(ps => ps.Service)
            //    .WithMany(s => s.Properties)
            //    .HasForeignKey(ps => ps.ServiceId)
            //    .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region PropertyType
            // Primary Key
            builder.Entity<PropertyType>().HasKey(pt => pt.Id);

            // Properties 
            builder.Entity<PropertyType>().Property(pt => pt.Name).HasMaxLength(100);
            builder.Entity<PropertyType>().Property(pt => pt.IconClass).HasMaxLength(50);

            // Relationship With Property
            builder.Entity<PropertyType>()
                .HasMany(pt => pt.Properties)
                .WithOne(p => p.PropertyType)
                .HasForeignKey(p => p.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Rating
            // Primary Key
            builder.Entity<Rating>().HasKey(r => r.Id);
            #endregion

            #region Service
            // Primary Key
            builder.Entity<Service>().HasKey(s => s.Id);
            builder.Entity<Service>()
                .HasOne(s => s.Property)
                .WithMany(p => p.Services)
                .HasForeignKey(s => s.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Properties 
            builder.Entity<Service>().Property(s => s.Name).HasMaxLength(100);
            #endregion

            #region User
            builder.Entity<User>().Property(u => u.Gender).HasConversion<string>().HasMaxLength(10);

            builder.Entity<User>().Property(u => u.Image).HasMaxLength(255);

            // Relationships With Hosted Properties 
            builder.Entity<User>()
                .HasMany(u => u.HostedProperties)
                .WithOne(p => p.Host)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships With Notifications
            builder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Add Roles in Identity Roles Table
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole()
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole()
                {
                    Id = "3",
                    Name = "Host",
                    NormalizedName = "HOST"
                }
            );
            #endregion
        }
        public virtual DbSet<Amenity> Amenities { get; set; }
        public virtual DbSet<AmenityCategory> AmenityCategories { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Favourite> Favourites { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyAmenity> PropertyAmenities { get; set; }
        //public virtual DbSet<PropertyService> PropertyServices { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }

    }
}
