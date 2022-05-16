using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class NotificationAccount
    {
        public int AccountId { get; set; }
        public int NotificationId { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual Account Account { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
