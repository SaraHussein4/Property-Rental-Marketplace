using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        //private readonly UserManager<User> _userManager;/
        private readonly PropertyDbContext _context; 
        public UserRepository(PropertyDbContext context):base(context) {
            //_userManager = userManager;
            _context = context;
        }

        public async Task<bool> CheckUserByPhone(string phone)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phone);
        }
        public async Task<User> GetUserByPhone(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone);
        }
        public async Task<User> GetImageUser (string id)
        {
            return await _context.Users.FirstOrDefaultAsync(w => w.Id == id);
        }
        public async Task<User> GetUserName( string id)
        {
            return await _context.Users.FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
