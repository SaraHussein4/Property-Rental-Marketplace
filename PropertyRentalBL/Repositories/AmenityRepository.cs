using Microsoft.EntityFrameworkCore;
using PropertyBL.Repositories;
using PropertyDAL.Contexts;
using PropertyRentalBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Repositories
{
    public class AmenityRepository: GenericRepository<Amenity>, IAmenityRepository
    {
        private readonly PropertyDbContext _context;
        public AmenityRepository(PropertyDbContext context) : base(context) {
            _context = context;
        }

        public async Task<IEnumerable<Amenity>> GetAmenities()
        {
            return await _context.Amenities.Where(a => a.AmenityCategory.Name == "Amenity").ToListAsync();
        }
        public async Task<IEnumerable<Amenity>> GetSafeties()
        {
            return await _context.Amenities.Where(a => a.AmenityCategory.Name == "Safety").ToListAsync();
        }
    }
}
