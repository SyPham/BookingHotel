using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class PartnerType
    {
        public PartnerType()
        {
            Partners = new HashSet<Partner>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? Position { get; set; }
        public bool? Status { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual ICollection<Partner> Partners { get; set; }
    }
}
