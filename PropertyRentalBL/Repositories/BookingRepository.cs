using Microsoft.EntityFrameworkCore;
using PropertyBL.Repositories;
using PropertyDAL.Contexts;
using PropertyDAL.Models;
using PropertyRentalBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        PropertyDbContext _context;
        public BookingRepository(PropertyDbContext context): base(context) { 
            _context = context;
        }

        public  async Task<List<Booking>> GetActiveBookingsForHost(string hostid)
        {
            //User host = await _context.Users.FirstOrDefaultAsync(u => u.Id ==hostid);
            //if (host!=null && host.BookingsHost!=null)
            //{
            //    return await host.sel
            //}
             return  await _context.Users.Where(u => u.Id == hostid).SelectMany(u => u.BookingsHost).ToListAsync();

            //throw new NotImplementedException();
        }

        
    }
}
