using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Country
    {
        public int Id {  get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double longitude {  get; set; }
        public double latitude { get; set; }
    }
}
