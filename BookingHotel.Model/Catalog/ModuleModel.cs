using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class ModuleModel
    {
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
    }
}
