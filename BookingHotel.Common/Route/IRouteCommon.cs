using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.Model.System.Commons;

namespace BookingHotel.Common.Route
{
    public partial interface IRouteCommon
    {
        List<ApiUrl> GetAllApiUrl();
        List<ActionName> GetAllAction();
    }
}
