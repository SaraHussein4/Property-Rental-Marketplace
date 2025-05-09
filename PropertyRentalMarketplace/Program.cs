using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PropertyBL.Interfaces;
using PropertyBL.Repositories;
using PropertyDAL.Contexts;
using PropertyDAL.Models;
using PropertyRentalBL.Interfaces;
using PropertyRentalBL.Repositories;
using PropertyRentalDAL.Models;
using Stripe;
using System.Configuration;

namespace PropertyRentalMarketplace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
            builder.Services.AddScoped<IPropertyAmenityRepository, PropertyAmenityRepository>();
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();


            builder.Services.AddIdentity<User, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = true;
                Options.Password.RequireDigit = true;
                Options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<PropertyDbContext>()
            .AddDefaultTokenProviders();
            //
           
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  
                 .AddCookie("UserScheme", options =>
                 {
                     options.LoginPath = "/Account/Login";
                     options.Cookie.Name = "UserAuthCookie";
                     options.AccessDeniedPath = "/Account/AccessDenied";
                 })
    .AddCookie("HostScheme", options =>
    {
        options.LoginPath = "/Account/Login";
        options.Cookie.Name = "HostAuthCookie";
        options.AccessDeniedPath = "/Account/AccessDenied";
    })
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthSection = builder.Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthSection["ClientId"];
                    options.ClientSecret = googleAuthSection["ClientSecret"];
                }

                );

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserPolicy", policy =>
                    policy.RequireRole("User")
                          .AddAuthenticationSchemes("UserScheme"));

                options.AddPolicy("HostPolicy", policy =>
                    policy.RequireRole("Host")
                          .AddAuthenticationSchemes("HostScheme"));
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<PropertyDbContext>(
                  optionBuilder =>
                  {
                      optionBuilder.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                  });


            var app = builder.Build();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
