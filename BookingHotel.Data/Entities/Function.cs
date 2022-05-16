using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Function
    {
        public Function()
        {
            AccountFunctions = new HashSet<AccountFunction>();
            FunctionGroupFunctions = new HashSet<FunctionGroupFunction>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public string Icon { get; set; }
        public int? Position { get; set; }
        public bool Status { get; set; }
        public bool? IsShow { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModuleId { get; set; }
        public int? ParentId { get; set; }

        public virtual Module Module { get; set; }
        public virtual ICollection<AccountFunction> AccountFunctions { get; set; }
        public virtual ICollection<FunctionGroupFunction> FunctionGroupFunctions { get; set; }
    }
}
