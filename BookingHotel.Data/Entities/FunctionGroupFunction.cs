using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class FunctionGroupFunction
    {
        public FunctionGroupFunction()
        {
        }

        public FunctionGroupFunction(int groupFunctionId, int functionId)
        {
            GroupFunctionId = groupFunctionId;
            FunctionId = functionId;
        }

        public int GroupFunctionId { get; set; }
        public int FunctionId { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public virtual Function Function { get; set; }
        public virtual GroupFunction GroupFunction { get; set; }
    }
}
