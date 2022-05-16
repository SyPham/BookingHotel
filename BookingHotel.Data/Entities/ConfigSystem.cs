using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class ConfigSystem
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Values { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string Description { get; set; }

        public virtual Account CreateByNavigation { get; set; }
    }
}
