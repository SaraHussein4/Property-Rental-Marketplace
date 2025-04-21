using PropertyBL.Repositories;
using PropertyDAL.Contexts;
using PropertyRentalBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Repositories
{
    public class LocationRepository: GenericRepository<Location>,ILocationRepository
    {
        private readonly PropertyDbContext _context;
        public LocationRepository(PropertyDbContext context): base(context) { 
            _context = context;
            Debug.WriteLine($"{context.GetHashCode()}");

        }
        //public test()
        //{
        //    Debug.WriteLine($"{_context.GetHash}")
        //}
    }
}
