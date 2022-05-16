using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class TabModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? Position { get; set; }
        public bool? IsDelete { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }

        public int? ProductId { get; set; }
        public string Value { get; set; }

        //public virtual ICollection<ProductTabModel> ProductTabs { get; set; }
    }
}
