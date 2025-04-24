using PropertyBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Interfaces
{
    public interface IImageRepository:IGenericRepository<Image>
    {
        public Task DeletePrimaryImageForProperty(int propertyId);

        public Task DeleteNonPrimaryImagesForProperty(int propertyId);
        public  Task<List<Image>> GetImageById(int propertyid);
      

    }
}
