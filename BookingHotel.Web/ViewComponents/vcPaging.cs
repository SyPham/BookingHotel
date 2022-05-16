using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.Web.ClassHelpers;

namespace BookingHotel.Web.ViewComponents
{
    public class vcPaging : ViewComponent
    {
        public IViewComponentResult Invoke(int page, int totalItems, int pageSize, string url, int maxPage = 10)
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            int pageFirst = 1;
            int pageLast = totalPages;

            int pageBack = page - 1;
            if (pageBack < 1)
                pageBack = 1;

            int pageNext = page + 1;
            if (pageNext > totalPages)
                pageNext = totalPages;

            string pageFirstUrl = string.Format(url, pageFirst);
            string pageLastUrl = string.Format(url, pageLast);
            string pageBackUrl = string.Format(url, pageBack);
            string pageNextUrl = string.Format(url, pageNext);

            PagingOutput output = new PagingOutput
            {
                Page = page,
                PageFirstUrl = pageFirstUrl,
                PageLastUrl = pageLastUrl,
                PageBackUrl = pageBackUrl,
                PageNextUrl = pageNextUrl
            };
            ViewBag.PagingOutput = output;




            Dictionary<int, string> pageNumbers = new Dictionary<int, string>();
            for (int i = 1; i <= totalPages; i++)
            {
                pageNumbers.Add(i, string.Format(url, i));
            }

            return View(pageNumbers);
        }
    }
}
