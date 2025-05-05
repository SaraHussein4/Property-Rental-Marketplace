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
      
        public async Task<List<Rating>> GetallRatingbyid(string id)
        {
            return await _context.Ratings.Where(w=>w.UserId == id).ToListAsync();
        }
        public async Task<List<Rating>> getallrating(string id)
        {
            return await _context.Ratings.Where(w => w.UserId == id).Select(
                s => new Rating
                {
                   AmenitiesRating= s.AmenitiesRating,
                    CommunicationRating= s.CommunicationRating,
                    OverallRating=  s.OverallRating,
                    ValueForMoneyRating= s.ValueForMoneyRating,
                    HygieneRating= s.HygieneRating,
                    LocationRating= s.LocationRating,
                }).ToListAsync();
        }
    }
}
