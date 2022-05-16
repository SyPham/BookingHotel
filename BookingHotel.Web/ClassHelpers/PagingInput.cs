using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingHotel.Web.ClassHelpers
{
    public class PagingInput
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int MaxPage { get; set; }
        public int TotalItems { get; set; }
        public string Url { get; set; }
    }
}
