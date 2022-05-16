using System;
using Serilog;
using Serilog.Events;
using BookingHotel.Common.Helpers;

namespace BookingHotel.ApplicationService.Loggings
{
    public partial class Logging : ILogging
    {
        #region Fields
        private readonly IWebHelper _webHelper;
        #endregion

        #region Ctor

        public Logging(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        #endregion

        #region Method

        public void LogException(Exception exception, object param)
        {
            string url = _webHelper.GetThisPageUrl(true);
            var logger = new LoggerConfiguration()
                         .MinimumLevel.Error()
                         .WriteTo.File("Logs/Exception/.txt",
                         LogEventLevel.Error, // Minimum Log level
                                 rollingInterval: RollingInterval.Day, // This will append time period to the filename like 20180316.txt
                                 retainedFileCountLimit: null,
                                 fileSizeLimitBytes: null,
                                 outputTemplate: "DateTime: {Timestamp:yyyy-MM-dd HH:mm:ss}{NewLine}{Message}{NewLine}Exception: {Exception}{NewLine}",  // Set custom file format
                                 shared: true // Shared between multi-process shared log files
                                 )
                         .CreateLogger();

            logger.Error(exception, "Url: {url}\r\nParam: {@param}", url, param);
        }
        #endregion
    }
}
