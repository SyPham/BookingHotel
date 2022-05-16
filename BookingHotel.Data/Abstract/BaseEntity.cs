using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookingHotel.Data.Abstract
{
   public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }

        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }

        public bool? IsDelete { get; set; }
    }
}
