using Microsoft.EntityFrameworkCore;
using PropertyBL.Repositories;
using PropertyDAL.Contexts;
using PropertyRentalBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Repositories
{
    public class ImageRepository:GenericRepository<Image>,IImageRepository
    {
        private readonly PropertyDbContext _context;

        public ImageRepository(PropertyDbContext context) : base(context) {
            _context = context;
            Debug.WriteLine($"{context.GetHashCode()}");
        }

        public async Task DeleteNonPrimaryImagesForProperty(int propertyId)
        {
            await _context.Images.Where(img => img.PropertyId == propertyId && img.IsPrimary == false).ExecuteDeleteAsync();
        }

        public async Task DeletePrimaryImageForProperty(int propertyId)
        {
            await _context.Images.Where(img => img.PropertyId == propertyId && img.IsPrimary == true).ExecuteDeleteAsync();
        }
        //get image by id
        public async Task<List<Image>> GetImageById(int propertyid)
        {
            return await _context.Images.Where(w => w.PropertyId == propertyid).ToListAsync();
        }
    }
}
