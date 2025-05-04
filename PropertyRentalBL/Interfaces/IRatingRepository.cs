using PropertyBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Interfaces
{
    public interface IRatingRepository:IGenericRepository<Rating>
    {
        public Task<List<Rating>> GetallRatingbyid(string id);
        public Task<List<Rating>> getallrating(string id);

    }
}
