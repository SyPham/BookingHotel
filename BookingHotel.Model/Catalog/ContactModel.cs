using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class ContactModel
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
    }
}
