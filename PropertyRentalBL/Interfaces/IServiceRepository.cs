﻿using Microsoft.EntityFrameworkCore;
using PropertyBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Interfaces
{
    public interface IServiceRepository: IGenericRepository<Service>
    {
        public  Task<List<Service>> GetAllServicesById(int id);
        
    }
}
