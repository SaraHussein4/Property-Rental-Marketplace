using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyRentalDAL.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool IsPrimary { get; set; }

        //Foreign Key
        public int PropertyId { get; set; }

        // Navigation Property
        public virtual Property Property { get; set; } //
    }
}
