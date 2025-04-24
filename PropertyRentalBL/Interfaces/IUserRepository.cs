using PropertyDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBL.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        //public Task<Property> GetImgHostById(int id); //
        public Task<string> getimagehost(int propertyid); //
    }
}
