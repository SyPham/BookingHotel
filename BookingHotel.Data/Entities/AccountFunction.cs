using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class AccountFunction
    {
        public AccountFunction()
        {
        }

        public AccountFunction(int accountId, int functionId)
        {
            AccountId = accountId;
            FunctionId = functionId;
        }
        public int FunctionId { get; set; }
        public int AccountId { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual Account Account { get; set; }
        public virtual Function Function { get; set; }
    }
}
