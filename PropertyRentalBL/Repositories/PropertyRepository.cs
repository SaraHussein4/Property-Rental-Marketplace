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

        // get image host

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

        public async Task<List<Property>> GetPropertyTypeById(int id)
        {
            var properties = await _context.Properties
                .Where(p => p.PropertyTypeId == id)
                .Include(p => p.Images)
                .ToListAsync();
            return properties;
        }



        public async Task<List<Property>> GetFilteredProperties(
int typeId,
List<string> priceRanges,
List<string> countries,
List<string> bedrooms)
        {
            
            var query = _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Location) 
                .Where(p => p.PropertyTypeId == typeId && p.IsListed)
                .AsQueryable();

           
            if (priceRanges != null && priceRanges.Any())
            {
                var priceFilterQueries = new List<IQueryable<Property>>();

                foreach (var range in priceRanges)
                {
                    if (range.Contains("+"))
                    {
                        var minPrice = decimal.Parse(range.Replace("+", ""));
                        priceFilterQueries.Add(query.Where(p => p.FeesPerMonth >= minPrice));
                    }
                    else if (range.Contains("-"))
                    {
                        var parts = range.Split('-');
                        var minPrice = decimal.Parse(parts[0]);
                        var maxPrice = decimal.Parse(parts[1]);
                        priceFilterQueries.Add(query.Where(p => p.FeesPerMonth >= minPrice && p.FeesPerMonth <= maxPrice));
                    }
                }

                query = priceFilterQueries.Aggregate((current, next) => current.Union(next));
            }

            if (countries != null && countries.Any())
            {
                query = query.Where(p => p.Location != null && countries.Contains(p.Location.Country));
            }

           
            if (bedrooms != null && bedrooms.Any())
            {
                var bedroomFilterQueries = new List<IQueryable<Property>>();

                foreach (var bed in bedrooms)
                {
                    if (bed.EndsWith("+"))
                    {
                        var minBeds = int.Parse(bed.Replace("+", ""));
                        bedroomFilterQueries.Add(query.Where(p => p.BedRooms >= minBeds));
                    }
                    else
                    {
                        var bedCount = int.Parse(bed);
                        bedroomFilterQueries.Add(query.Where(p => p.BedRooms == bedCount));
                    }
                }

               
                query = bedroomFilterQueries.Aggregate((current, next) => current.Union(next));
            }

            
            query = query.Where(p => p.UnListDate > DateTime.Now);

            return await query
                .OrderByDescending(p => p.IsFeatured) 
                .ThenByDescending(p => p.ListedAt)    
                .ToListAsync();
        }

        public async Task<double> GetStarRating(int propertyid)
        {
            Property property = await _context.Properties.FirstOrDefaultAsync(property => property.Id == propertyid);
            return property.Bookings
                .Where(booking => booking.Rating != null && booking.Rating.OverallRating != null)
                .Average(booking => booking.Rating.OverallRating);
        }



        public async Task<IEnumerable<Property>> GetTopRating1()
        {
            return await _context.Properties.OrderByDescending(p=>p.StarRating).ToListAsync();
        }

    }

}
