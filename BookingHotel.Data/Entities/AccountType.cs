using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class AccountType
    {
        public AccountType()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool? Status { get; set; }
        public int? Position { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string Key { get; set; }
        public bool? IsDelete { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
