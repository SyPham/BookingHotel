using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingHotel.Common.Helpers
{
    public partial class WebHelper : IWebHelper
    {
        #region Fields 
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public WebHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Ulities

        /// <summary>
        /// Check whether current HTTP request is available
        /// </summary>
        /// <returns>True if available; otherwise false</returns>
        protected virtual bool IsRequestAvailable()
        {
            if (_httpContextAccessor?.HttpContext == null)
                return false;

            try
            {
                if (_httpContextAccessor.HttpContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Method

        public string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //add local path to the URL
            var pageUrl = $"{_httpContextAccessor.HttpContext.Request.Path}";

            //add query string to the URL
            if (includeQueryString)
                pageUrl = $"{pageUrl}{_httpContextAccessor.HttpContext.Request.QueryString}";

            //whether to convert the URL to lower case
            if (lowercaseUrl)
                pageUrl = pageUrl.ToLowerInvariant();

            return pageUrl;
        }

        #endregion
    }
}
