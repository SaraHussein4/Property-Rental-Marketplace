using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using PropertyBL.Interfaces;
using PropertyDAL.Contexts;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly PropertyDbContext context;
        private IDbContextTransaction transaction;
        public GenericRepository(PropertyDbContext _context)
        {
            context = _context;
        }
        public async Task BeginTransactionAsync()
        {
            transaction = await context.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            await transaction.CommitAsync();
        }
        public async Task RollbackAsync()
        {
            await transaction.RollbackAsync();
        }

        public async Task Add(T item)
        {
            await context.AddAsync(item);
        }

        public async Task Delete(T item)
        {
            context.Remove(item);        
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }
        //get image by id
        public async Task<List<Image>> GetImageById(int propertyid)
        {
            return await context.Images.Where(w => w.PropertyId == propertyid).ToListAsync();
        }
        // get image host
        
        public async Task<string> getimagehost(int propertyid)
        {
            return await context.Images.Where(w => w.PropertyId == propertyid).Select(s => s.Path).FirstOrDefaultAsync();

        }
        //Amenities 
        public async Task<List<Amenity>> GetAllAmenitiesById(int propertyid)
        {
            return await context.Amenities.Where(w=>w.Id == propertyid).ToListAsync();
        }
        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }

        public async Task Update(T item)
        {
            context.Update(item);
        }
    }
}
