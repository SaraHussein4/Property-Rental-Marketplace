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
    public class RatingRepository: GenericRepository<Rating>, IRatingRepository 
    {
        private readonly PropertyDbContext _context;
        public RatingRepository(PropertyDbContext context):base(context) {
            _context = context;
        }
        public async Task<List<Rating>> GetallRatingbyid(int idbook)
        {
            return await _context.Ratings.Where(w=>w.Id == idbook).ToListAsync();
        }
    }
}
