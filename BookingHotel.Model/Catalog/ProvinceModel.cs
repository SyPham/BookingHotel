using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class ProvinceModel
    {
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
    }
}
