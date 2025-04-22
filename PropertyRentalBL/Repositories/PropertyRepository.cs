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
    public class PropertyRepository: GenericRepository<Property>, IPropertyRepository
    {
        PropertyDbContext _context;
        public PropertyRepository(PropertyDbContext context) : base(context) {
            _context = context;
        }

        public Task<List<Property>> GetActiveListedPropertiesHostedBySpecificHost(string hostId)
        {
            return _context.Properties.AsNoTracking().Where(p => p.UserId == hostId && p.UnListDate > DateTime.Now).ToListAsync();
        }
    }
    
}
