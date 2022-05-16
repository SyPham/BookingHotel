using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Common.Helpers
{
    public partial interface IWebHelper
    {
        /// <summary>
        /// Gets this page URL
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
        /// <param name="useSsl">Value indicating whether to get SSL secured page URL. Pass null to determine automatically</param>
        /// <param name="lowercaseUrl">Value indicating whether to lowercase URL</param>
        /// <returns>Page URL</returns>
        string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false);
    }
}
