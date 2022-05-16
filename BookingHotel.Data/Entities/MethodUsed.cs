using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class MethodUsed
    {
        public MethodUsed()
        {
            Products = new HashSet<Product>();
            ProductWallets = new HashSet<ProductWallet>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModifyBy { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductWallet> ProductWallets { get; set; }
    }
}
