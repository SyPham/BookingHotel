using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Province
    {
        public Province()
        {
            Districts = new HashSet<District>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal? FeeShip { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public bool? Status { get; set; }
        public bool? IsDelete { get; set; }

        public virtual Account CreateByNavigation { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
