using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PropertyDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int FeePerMonth { get; set; }
        public bool IsActive { get; set; }

        //Foreign Key
        public string UserId { get; set; }
        public string HostId{ get; set; }

        public int PropertyId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; } //
        public virtual User Host { get; set; }
        public virtual Property Property { get; set; } //
        public virtual Rating Rating { get; set; } //
    }
}
