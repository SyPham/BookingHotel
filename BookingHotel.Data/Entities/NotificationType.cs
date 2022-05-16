using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notifications = new HashSet<Notification>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool? Status { get; set; }
        public int? Position { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
