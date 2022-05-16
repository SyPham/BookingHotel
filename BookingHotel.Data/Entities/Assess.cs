using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Assess
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public string Phone{ get; set; }
        public string Note { get; set; }
        public decimal? NumberStar { get; set; }
        public int? KeyId { get; set; }
        public string KeyName { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
