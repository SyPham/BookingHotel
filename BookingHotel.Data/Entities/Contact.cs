using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Contact
    {
        public int Id { get; set; }
        public int? ApproveBy { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Mobi { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual Account ApproveByNavigation { get; set; }
    }
}
