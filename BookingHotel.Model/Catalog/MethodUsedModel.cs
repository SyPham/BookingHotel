using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class MethodUsedModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }

        //public virtual ICollection<ProductModel> Products { get; set; }
        //public virtual ICollection<ProductWalletModel> ProductWallets { get; set; }
    }
}
