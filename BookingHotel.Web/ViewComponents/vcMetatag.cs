using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingHotel.Web.ClassHelpers;

namespace BookingHotel.Web.ViewComponents
{
    public class vcMetatag : ViewComponent
    {
        private string keywordContent = "Baby Monkey, đồ trẻ em, đồ trẻ em khuyến mãi, đồ trẻ em cao cấp, đồ trẻ em bình dân. ";
        private string descriptionContent = "Baby Monkey là đơn vị tiên phong tại Việt Nam chuyên về ngành hàng dành riêng cho trẻ em.";
      /// <summary>
      /// Dùng để chèn thẻ meta vào head
      /// </summary>
      /// <param name="keyWord">Từ khóa SEO</param>
      /// <param name="description">Mô tả SEO</param>
      /// <returns>Các thẻ meta</returns>
        public IViewComponentResult Invoke(string keyWord, string description)
        {
            keyWord = string.IsNullOrEmpty(keyWord) ? keywordContent : keyWord;
            description = string.IsNullOrEmpty(description) ? descriptionContent : description;
            string metatags = string.Empty;
            metatags += @$"<meta name='keyWords' content='{keyWord}' />";
            metatags += @$"<meta name='description' content='{description}' />";
            metatags += @$"<meta name='author' content='Henry Pham' />";
            metatags += $@"<meta http-equiv=”content-language” content=”vi” />";
            metatags += $@"
                            <meta name='DC.title' content='Baby Monkey' />
                            <meta name='geo.region' content='VN-SG' />
                            <meta name='geo.placename' content='Hồ Ch&iacute; Minh' />
                            <meta name='geo.position' content='13.2904;108.42651' />
                            <meta name='ICBM' content='13.2904, 108.42651' />
                            ";
            ViewBag.Metatags = metatags;
            return View();
        }
    }
}
