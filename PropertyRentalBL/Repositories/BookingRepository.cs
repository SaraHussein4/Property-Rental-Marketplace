using PropertyBL.Repositories;
using PropertyDAL.Contexts;
using PropertyRentalBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Repositories
{
    public class BookingRepository: GenericRepository<Booking>, IBookingRepository
    {
        PropertyDbContext _context;
        public BookingRepository(PropertyDbContext context): base(context) { 
            _context = context;
        }

    }
}
