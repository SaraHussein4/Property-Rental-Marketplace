using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBL.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task Add(T item);
        Task Update(T item);
        Task Delete(T item);
        Task<int> Save();
        Task<T> GetById(int id);
        Task<List<Image>> GetImageById(int propertyid);
       Task<string> getimagehost(int propertyid);//
        public  Task BeginTransactionAsync();
        public Task CommitAsync();
        public Task RollbackAsync();

    }
}
