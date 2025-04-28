using Microsoft.EntityFrameworkCore;
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
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly PropertyDbContext _context;

        public NotificationRepository(PropertyDbContext context):base(context) { 
            _context = context;
        }

        public async Task<Notification> GetNotificationForUserAndBooking(string userId, int bookingId)
        {
            return await _context.Notifications.Where(n => n.User.Id == userId && n.BookingId == bookingId).FirstOrDefaultAsync();
        }

        public async Task<List<Notification>> GetNotificationsForUser(string userId)
        {
            return await _context.Notifications.Where(n => n.User.Id == userId && !n.IsReaded).ToListAsync();
        }
    }
}
