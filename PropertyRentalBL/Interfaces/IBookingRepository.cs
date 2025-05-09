﻿using PropertyBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Interfaces
{
    public interface IBookingRepository: IGenericRepository<Booking>
    {
        public Task<List<Booking>> GetActiveBookingsForHost(string hostid);
    }
}
