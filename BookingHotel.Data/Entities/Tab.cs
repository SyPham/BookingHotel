using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Tab
    {
        public Tab()
        {
            ProductTabs = new HashSet<ProductTab>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? Position { get; set; }
        public bool? IsDelete { get; set; }
        public bool? Status { get; set; }
        public int? TabTypeId { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }

        public virtual TabType TabType { get; set; }

        public virtual ICollection<ProductTab> ProductTabs { get; set; }
    }
}
