using Microsoft.EntityFrameworkCore;
using PropertyBL.Repositories;
using PropertyDAL.Contexts;
using PropertyRentalBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            var exist = await _context.Favourites.Where(w => w.UserId == UId && w.PropertyId == PropId).FirstOrDefaultAsync();
            if (exist == null)
            {
                var favourute = new Favourite
                {
                    UserId = UId,
                    PropertyId = PropId,
                };
                await _context.AddAsync(favourute);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Favourite>> getallfav(string id)
        {
            return await _context.Favourites.Where(w => w.UserId == id).ToListAsync();
        }
        public async Task RemoveToFavourite(string UId, int PropId)
        {
            var exist = await _context.Favourites.FirstOrDefaultAsync(w => w.UserId == UId && w.PropertyId == PropId);
            if (exist != null)
            {
                _context.Remove(exist);
                    await _context.SaveChangesAsync();
               
            }
           
        }
        public async Task<Favourite> removefav(string id, int propid)
        {
            return await _context.Favourites.Where(w => w.UserId == id && w.PropertyId == propid).FirstOrDefaultAsync();
        }
       
        public async Task<List<Favourite>> getallfavtoremove(string id, int propid)
        {
            return await _context.Favourites.Where(w => w.UserId == id && w.PropertyId == propid).ToListAsync();
        }
        //public async Task<bool> isfav(string id, int propid)
        //{
        //   return await _context.Favourites.AnyAsync(a => a.UserId == id && a.PropertyId == propid);
        //}
        
    }
}
