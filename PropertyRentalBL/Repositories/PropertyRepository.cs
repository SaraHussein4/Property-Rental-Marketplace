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
        private readonly PropertyDbContext _context;
        public PropertyRepository(PropertyDbContext context) : base(context) {
        _context = context;
        
        }

        public async Task<IEnumerable<Property>> GetAllFeatured()
        {
            return await _context.Properties.Where(p=>p.IsFeatured==true).ToListAsync();
        }
        // get image host

        public async Task<string> getimagehost(int propertyid)
        {
            return await _context.Properties.Where(w => w.Id == propertyid).Select(s => s.Host.Image).FirstOrDefaultAsync();

        }
       
    }
    
}
