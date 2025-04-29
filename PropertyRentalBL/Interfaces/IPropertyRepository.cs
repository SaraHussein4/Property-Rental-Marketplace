using Microsoft.EntityFrameworkCore;
using PropertyBL.Interfaces;
using PropertyBL.Repositories;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<IEnumerable<Property>> GetAllFeatured();

        public Task<string> getimagehost(int propertyid); //
        public Task<List<Property>> GetActiveListedPropertiesHostedBySpecificHost(string hostId);
        public Task<List<Property>> GetExpiredPropertiesHostedBySpecificHost(string hostId);

        public Task<List<Property>> GetPropertyTypeById(int id);

        public Task<List<Property>> GetFilteredProperties(
int typeId,
List<string> priceRanges,
List<string> countries,
List<string> bedrooms);


        
    }
}
