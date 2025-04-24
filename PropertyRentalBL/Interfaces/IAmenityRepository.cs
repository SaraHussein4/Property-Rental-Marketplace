using PropertyBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Interfaces
{
    public interface IAmenityRepository: IGenericRepository<Amenity>
    {
        public Task<IEnumerable<Amenity>> GetAmenities();
        public Task<IEnumerable<Amenity>> GetSafeties();
        public Task<List<Amenity>> GetAllAmenitiesById(int propertyid);
    }
}
