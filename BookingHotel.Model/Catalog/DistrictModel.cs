using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class DistrictModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public int? ProvinceId { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public bool? Status { get; set; }
        public bool? IsDelete { get; set; }
    }
}
