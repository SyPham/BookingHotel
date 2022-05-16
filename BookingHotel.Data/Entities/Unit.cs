using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Unit
    {
        public Unit()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
