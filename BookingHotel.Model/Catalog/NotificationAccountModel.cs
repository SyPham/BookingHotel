using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class NotificationAccountModel
    {
        public int AccountId { get; set; }
        public int NotificationId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
