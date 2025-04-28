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
    public class FavouriteRepository : GenericRepository<Favourite> , IFavouriteRepository
    {
        private readonly PropertyDbContext _context;

        public FavouriteRepository(PropertyDbContext context) : base(context)
        {
            _context = context;

        }
        public async Task AddToFavourite(string UId, int PropId)
        {
            var favourute = new Favourite
            {
                UserId = UId,
                PropertyId = PropId,
            };
            await _context.AddAsync(favourute);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveFromFavourite(string UId, int PropId)
        {
            var fav= await _context.Favourites.Where(w=>w.UserId == UId && w.PropertyId == PropId).FirstOrDefaultAsync();
            if (fav != null)
            {
                 _context.Favourites.Remove(fav);
                await _context.SaveChangesAsync();
            }
        }
    }
}
