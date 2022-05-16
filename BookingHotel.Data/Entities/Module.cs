using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Module
    {
        public Module()
        {
            Functions = new HashSet<Function>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Position { get; set; }
        public string Icon { get; set; }
        public bool? Status { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual ICollection<Function> Functions { get; set; }
    }
}
