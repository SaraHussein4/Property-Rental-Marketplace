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
    public class RatingRepository: GenericRepository<Rating>, IRatingRepository 
    {
        public RatingRepository(PropertyDbContext context):base(context) { }
    }
}
