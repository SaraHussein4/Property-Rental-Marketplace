using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyDAL.Contexts
{
    public class PropertyDbContext : IdentityDbContext <ApplicationUser>    
    {
        public PropertyDbContext(DbContextOptions<PropertyDbContext> options):base(options) { }

        public virtual DbSet<User> Users { get; set; }
       
    }
}
