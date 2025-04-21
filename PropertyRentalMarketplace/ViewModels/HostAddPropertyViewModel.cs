using Microsoft.AspNetCore.Mvc.Rendering;
using PropertyRentalDAL.Models;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalMarketplace.ViewModels
{
    public class HostAddPropertyViewModel
    {
        public IEnumerable<PropertyType>? propertyTypes { get; set; }
        public IEnumerable<Amenity>? amenities {  get; set; }
        public IEnumerable<Amenity>? safeties {  get; set; }
        public IEnumerable<Country>? countries {  get; set; }
        public IEnumerable<ListingTypeViewModel>? listingTypes {  get; set; }

        [Required(ErrorMessage = "Listing type is required")]
        [Range(1, 4, ErrorMessage = "Please enter between 0-4 pets")]

        public int ListingType { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Name { get; set; } //


        [Required(ErrorMessage = "Description is required")]
        [MaxLength(3000, ErrorMessage = "Description cannot exceed 2000 characters")]
        [MinLength(1000, ErrorMessage = "Description cannot be less than 1000 characters")]
        public string Description { get; set; } //

        [Required(ErrorMessage = "Property type is required")]
        public int PropertyTypeId { get; set; } // 

        [Required(ErrorMessage = "Number of Bedrooms is required")]
        [Range(0, 11, ErrorMessage = "Please enter between 0-11 Bedrooms")]
        public int Bedrooms { get; set; } //
        
        [Required(ErrorMessage = "Number of Bathrooms is required")]
        [Range(0, 11, ErrorMessage = "Please enter between 0-11 Bathrooms")]
        public int Bathrooms { get; set; } //
        
        [Required(ErrorMessage = "Number of Parking slots is required")]
        [Range(0, 6, ErrorMessage = "Please enter between 0-6 parking slots")]
        public int GarageSlots { get; set; } //

        [Required(ErrorMessage = "Number of pets allowed is required")]
        [Range(0, 4, ErrorMessage = "Please enter between 0-4 pets")]
        public int BetsAllowed { get; set; } //

        [Required(ErrorMessage = "Square footage is required")]
        [Range(0, 100000, ErrorMessage = "Area must be positive")]
        public int Area { get; set; } //


        [Required(ErrorMessage = "Monthly rent is required")]
        [Range(0, 100000, ErrorMessage = "Rent must be between $0 and $100,000")]
        public int FeesPerMonth { get; set; } //


        public string CountryCode { get; set; }
        //public string CityCode { get; set; }
        public string StateCode { get; set; }

        [Required(ErrorMessage = "specifie your property location on the map")]
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }

        public List<ServiceViewModel> Services { get; set; }

        //public string? NearbyServiceName1 { get; set; }
        //public string? NearbyServiceDistance1 { get; set; }
        //public string? NearbyServiceRate1 { get; set; }

        //public string? NearbyServiceName2 { get; set; }
        //public string? NearbyServiceDistance2 { get; set; }
        //public string? NearbyServiceRate2 { get; set; }

        public IEnumerable<int>? Amenities { get; set; }
        public IEnumerable<int>? Safeties { get; set; }


        [Required(ErrorMessage = "Cover image is required")]
        [DataType(DataType.Upload)]
        public IFormFile PrimaryImage { get; set; }
        public string? PrimaryImageUrl {  get; set; }

        [Required(ErrorMessage = "At least 5 property image is required")]
        [DataType(DataType.Upload)]
        public List<IFormFile> Images { get; set; }
        public List<string>? ImagesUrls {  get; set; }


        public int ListingPlan { get; set; }
        public string PaymentMethod {  get; set; }

    }
}
