using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookingHotel.Data.Abstract
{
   public abstract class CommonEntity
    {
        public string CreateBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreateTime { get; set; }

        public string ModifyBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifyTime { get; set; }

    }
}
