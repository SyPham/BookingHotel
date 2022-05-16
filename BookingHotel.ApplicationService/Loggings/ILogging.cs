using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.ApplicationService.Loggings
{
    public partial interface ILogging
    {
        /// <summary>
        /// create log exception
        /// </summary>
        /// <param name="exception">Error message</param>
        /// <param name="param">param value</param>
        void LogException(Exception exception, object param);
    }
}
