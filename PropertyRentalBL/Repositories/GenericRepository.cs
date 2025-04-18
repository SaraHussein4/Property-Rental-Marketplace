using PropertyBL.Interfaces;
using PropertyDAL.Contexts;
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
        public GenericRepository(PropertyDbContext _context)
        {
            context = _context;
        }
        public void Add(T item)
        {
            context.Add(item);
        }

        public void Delete(T item)
        {
            context.Remove(item);        
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public void Update(T item)
        {
            context.Update(item);
        }
    }
}
