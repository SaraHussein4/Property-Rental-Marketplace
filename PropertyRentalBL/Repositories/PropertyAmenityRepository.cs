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
    public class PropertyAmenityRepository:GenericRepository<PropertyAmenity>,IPropertyAmenityRepository
    {
        private readonly PropertyDbContext _context;

        public PropertyAmenityRepository(PropertyDbContext context):base(context)
        {
            _context = context;
        }

        public async Task DeleteAmenitiesForProperty(int propertyId)
        {
            await _context.PropertyAmenities.Where(pa => pa.PropertyId == propertyId).ExecuteDeleteAsync();
        }
    }
}
