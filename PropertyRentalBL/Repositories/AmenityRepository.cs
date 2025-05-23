﻿using Microsoft.EntityFrameworkCore;
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
        //Amenities 
        public async Task<List<Amenity>> GetAllAmenitiesById(int propertyid)
        {
            return await _context.PropertyAmenities.Where(w=>w.PropertyId == propertyid)
                .Select(s=>s.Amenity).Where(d=>d.AmenityCategoryId == 1).ToListAsync();
        }
        //Safety
        public async Task<List<Amenity>> GetSafetyById(int propertyid)
        {
            return await _context.PropertyAmenities.Where(w => w.PropertyId == propertyid)
                .Select(s => s.Amenity).Where(d => d.AmenityCategoryId == 2).ToListAsync();
        }
    }
}
