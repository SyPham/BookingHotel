using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class AccountGroupAccount
    {
        public int AccountId { get; set; }
        public int GroupAccountId { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public bool? IsDefault { get; set; }

        public virtual Account Account { get; set; }
        public virtual GroupAccount GroupAccount { get; set; }
    }
}
