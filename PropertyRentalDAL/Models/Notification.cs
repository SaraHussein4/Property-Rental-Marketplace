using PropertyDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Key
        public string UserId { get; set; }

        // Navigation Property
        public virtual User User { get; set; } //
    }
}
