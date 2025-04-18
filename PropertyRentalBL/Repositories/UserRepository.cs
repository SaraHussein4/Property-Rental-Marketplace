using PropertyBL.Interfaces;
using PropertyDAL.Contexts;
using PropertyDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBL.Repositories
{
    public class UserRepository:GenericRepository<User> , IUserRepository
    {
        public UserRepository(PropertyDbContext context):base(context) { }  
       
    }
}
