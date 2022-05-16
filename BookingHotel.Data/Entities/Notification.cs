using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Notification
    {
        public Notification()
        {
            NotificationAccounts = new HashSet<NotificationAccount>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int? NotificationTypeId { get; set; }
        public string Receiver { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public bool? Status { get; set; }

        public virtual Account CreateByNavigation { get; set; }
        public virtual NotificationType NotificationType { get; set; }

        public virtual ICollection<NotificationAccount> NotificationAccounts { get; set; }
    }
}
