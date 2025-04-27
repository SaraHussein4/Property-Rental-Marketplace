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
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly PropertyDbContext _context;
        public PropertyRepository(PropertyDbContext context) : base(context) {
        _context = context;
        
        }

        public async Task<IEnumerable<Property>> GetAllFeatured()
        {
            return await _context.Properties.Where(p=>p.IsFeatured==true).ToListAsync();
        }
        public async Task<string> getimagehost(int propertyid)
        {
            return await _context.Properties.Where(w => w.Id == propertyid).Select(s => s.Host.Image).FirstOrDefaultAsync();
        }

        public async Task<List<Property>> GetActiveListedPropertiesHostedBySpecificHost(string hostId)
        {
            return await _context.Properties.AsNoTracking().Where(p => p.UserId == hostId && p.IsListed == true && p.UnListDate > DateTime.Now).ToListAsync();
        }

        public async Task<List<Property>> GetExpiredPropertiesHostedBySpecificHost(string hostId)
        {
            var wasListed = await _context.Properties
                .Where(p => p.UserId == hostId && p.IsListed == true && p.UnListDate <= DateTime.Now)
                .ToListAsync();

            var wasBooked = await _context.Properties
                .Include(p => p.Bookings)
                .Where(p => p.UserId == hostId &&
                            p.IsListed == false &&
                            p.Bookings.Any() && 
                            p.Bookings.OrderByDescending(b => b.EndDate).First().EndDate <= DateTime.Now)
                .ToListAsync();

            return wasListed.Union(wasBooked).ToList();
        }
    }
    
}
