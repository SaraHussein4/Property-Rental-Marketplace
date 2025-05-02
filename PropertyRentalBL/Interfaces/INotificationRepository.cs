using PropertyBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Interfaces
{
    public interface INotificationRepository: IGenericRepository<Notification>
    {
        public Task<List<Notification>> GetNotificationsForUser(string userId);
        public Task<Notification> GetNotificationForUserAndBooking(string userId, int bookingId);

    }
}
