using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingHotel.Model.System.Commons;

namespace BookingHotel.Common.Route
{
    public class RouteCommon : IRouteCommon
    {

        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        public RouteCommon(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public List<ApiUrl> GetAllApiUrl()
        {
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(
              ad => ad.AttributeRouteInfo != null).Select(ad => new ApiUrl
              {
                  Controller = ad.RouteValues["controller"],
                  Action = ad.RouteValues["action"],
                  Url = ad.AttributeRouteInfo.Template
              }).ToList();
            return routes;
        }

        public List<ActionName> GetAllAction()
        {
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(
               ad => ad.AttributeRouteInfo == null).Select(ad => new ActionName
               {
                   Controller = ad.RouteValues["controller"],
                   Action = ad.RouteValues["action"]
               }).ToList();
            return routes;
        }
    }
}
