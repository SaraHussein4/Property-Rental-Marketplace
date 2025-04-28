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
        public Task RemoveFromFavourite(string UId, int PropId);


    }
}
