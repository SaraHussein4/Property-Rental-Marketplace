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
        public Task<bool> CheckUserByPhone(string phone);
        public Task<User> GetUserByPhone(string phone);
    }
}
