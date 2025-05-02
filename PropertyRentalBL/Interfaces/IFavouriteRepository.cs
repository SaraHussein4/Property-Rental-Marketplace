using PropertyBL.Interfaces;
using PropertyRentalDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalBL.Interfaces
{
    public interface IFavouriteRepository:IGenericRepository<Favourite>
    {
        public  Task AddToFavourite(string UId, int PropId);
        //public Task RemoveFromFavourite(string UId, int PropId);
        public  Task<List<Favourite>> getallfav(string id);
        public Task<Favourite> removefav(string id, int propid);
        public  Task<List<Favourite>> getallfavtoremove(string id, int propid);
        public  Task RemoveToFavourite(string UId, int PropId);
    }
}
