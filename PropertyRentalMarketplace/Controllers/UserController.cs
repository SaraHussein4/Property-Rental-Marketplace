using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyBL.Interfaces;
using PropertyDAL.Models;
using PropertyRentalBL.Interfaces;
using PropertyRentalBL.Repositories;
using PropertyRentalDAL.Enumerates;
using PropertyRentalDAL.Models;
using PropertyRentalMarketplace.ViewModels;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace PropertyRentalMarketplace.Controllers
{

    //[Authorize]
    public class UserController : Controller
    {
        // IUserRepository
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IImageRepository _imageRepository;

        public UserController(IPropertyRepository propertyRepository
            , IFavouriteRepository favouriteRepository ,IImageRepository imageRepository)
        {
            _propertyRepository = propertyRepository;
            _favouriteRepository = favouriteRepository;
            _imageRepository = imageRepository;
        }
        #region index
        //public async Task<IActionResult> Index()
        //{
        //    var images = new List<Image>
        //    {
        //        new Image { Id = 1, Path = "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
        //        new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
        //        new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
        //        new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
             
        //    };
        //    var images2 = new List<Image>
        //    {
        //       new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 2},
        //        new Image { Id = 2, Path= "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 2}
        //    };
        //    var property = new PropertyViewModel()
        //        {
        //            Id = 1,
        //            Name = "Name",
        //            Address = "LLL",
        //            BedRooms = 3,
        //            BathRooms = 2,
        //            BetsAllowd = 0,
        //            GarageSlots = 1,
        //            IsListed = true,
        //            IsFeatured = false,
        //            ListedAt = DateTime.Now,
        //            UnListDate = (DateTime.Now).AddDays(12),
        //            ListingType = ListingType.Rent,
        //            Images=images

        //        };
        //    var property1 = new PropertyViewModel()
        //    {
        //        Id = 2,
        //        Name = "Name",
        //        Address = "LLL",
        //        BedRooms = 3,
        //        BathRooms = 2,
        //        BetsAllowd = 0,
        //        GarageSlots = 1,
        //        IsListed = true,
        //        IsFeatured = true,
        //        ListedAt = DateTime.Now,
        //        UnListDate = (DateTime.Now).AddDays(12),
        //        ListingType = ListingType.Rent,
        //        Images = images2
        //    };


        //    var allProperities = new List<PropertyViewModel> { property ,property1};
        //    var featuedModel = allProperities.Where(model=>model.IsFeatured==true).ToList();
        //    var model = new PropertyPageViewModel
        //    {
        //        AllProperities = allProperities,
        //        FeaturedProperities = featuedModel
        //    };
        //    return View(model);
        //}
        #endregion
        #region details
        public async Task<IActionResult> Details(int id)
        {
            //var images = new List<Image>
            //{
            //    new Image { Id = 1, Path = "https://i.pinimg.com/736x/c6/be/a0/c6bea0070d17659af2f8856e4a627e6c.jpg", PropertyId = 1 },
            //    new Image { Id = 2, Path= "https://images.pexels.com/photos/265004/pexels-photo-265004.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
            //    new Image { Id = 3, Path= "https://images.pexels.com/photos/271624/pexels-photo-271624.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
            //    new Image { Id = 4, Path= "https://images.pexels.com/photos/1918291/pexels-photo-1918291.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
            //    new Image { Id = 5, Path= "https://images.pexels.com/photos/1643384/pexels-photo-1643384.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
            //    new Image { Id = 6, Path= "https://images.pexels.com/photos/259962/pexels-photo-259962.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
            //    new Image { Id = 7, Path= "https://images.pexels.com/photos/1457847/pexels-photo-1457847.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
            //    new Image { Id = 8, Path= "https://images.pexels.com/photos/1571453/pexels-photo-1571453.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
            //    new Image { Id = 9, Path= "https://images.pexels.com/photos/265004/pexels-photo-265004.jpeg?auto=compress&cs=tinysrgb&w=600", PropertyId = 1 },
            //    new Image { Id = 10, Path= "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBU3UXoeGSiQtW0no3PdFq_g_rKEWKS9flyw&s", PropertyId = 1 }

            //};
            var property = await _propertyRepository.GetById(1);
            if (property == null)
            {
                return View("Error");
            }
            var images = await _imageRepository.GetAll();
            var imagesVM = images.Select(s => new ImageViewModel
            {
                Id = s.Id,
                Path = s.Path,
                IsPrimary = s.IsPrimary,
            }).ToList();
            //var imagesVM = new List<ImageViewModel>
            //{
            //    new ImageViewModel{
            //    Id = images.Id,
            //    Path = images.Path,
            //    IsPrimary=images.IsPrimary,
            //    }

            //};

            var propertyVm = new PropertyViewModel()
            {
                Id = property.Id,
                Name = property.Name,
                Address = property.Address,
                Description = property.Description,
                BedRooms = property.BedRooms,
                BathRooms = property.BathRooms,
                BetsAllowd = property.BetsAllowd,
                GarageSlots = property.GarageSlots,
                IsListed = property.IsListed,
                IsFeatured = property.IsFeatured,
                ListedAt = property.ListedAt,
                UnListDate = property.UnListDate,
                ListingType = property.ListingType,
               Images= imagesVM

            };
           


           var model = new List<PropertyViewModel> { propertyVm };
            if(model == null)
            {
                return View("Error");
            }
                return View("Details" ,property);
        }
        #endregion
    }
}
