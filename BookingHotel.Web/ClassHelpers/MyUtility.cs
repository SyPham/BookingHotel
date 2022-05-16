using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookingHotel.Common.Helpers;

namespace BookingHotel.Web.ClassHelpers
{
    public static class MyUtility
    {
        public static bool IsPhoneNumber(this string number)
        {
            var check = string.IsNullOrWhiteSpace(number);
            if (check == false) return check;
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
        public static string StripUnicodeCharactersFromString(string inputValue)
        {
            StringBuilder newStringBuilder = new StringBuilder();
            newStringBuilder.Append(inputValue.Normalize(NormalizationForm.FormKD).Where(x => x < 128).ToArray());
            return newStringBuilder.ToString();
        }
        public static string ToImage(this string url, string baseUrl)
        {
            var value = baseUrl + url;
            if ( !value.IsExistImage()) return "/images/1.jpg";
            return value;
        }
        public static string ToProductImage(this string url, string baseUrl)
        {
            var value = baseUrl + url;
            if (!value.IsExistImage()) return "/images/no-product.png";
            return value;
        }
        public static string ToCenterBannerImage(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if (!value.IsExistImage()) return "/images/no-center-banner.png";
            return value;
        }
        public static string ToBigSubBannerImage(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if (!value.IsExistImage()) return "/images/no-big-sub-banner.png";
            return value;
        }
        public static string ToSmallSubBannerImage(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if (!value.IsExistImage()) return "/images/no-small-sub-banner.png";
            return value;
        }
        public static string ToMainBannerImage(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if (!value.IsExistImage()) return "/images/no-main-banner.png";
            return value;
        }
        public static string ToImageBanner(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if ( !value.IsExistImage()) return "/images/no-banner.png";
            return value;
        }
        public static string ToImageBannerRight(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if ( !value.IsExistImage()) return "/images/no-img-banner-right.jpg";
            return value;
        }
        public static string ToImageBrand(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if ( !value.IsExistImage()) return "/images/no-brand.png";

            return value;
        }
        public static string ToImageCat(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if ( !value.IsExistImage()) return "/images/no-img-cat.jpg";
            return value;
        }
        public static string ToBlogImage(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if (!value.IsExistImage()) return "/images/no-blog.png";
            return value;
        }
        public static string ToImage(this string url)
        {
            if ( !url.IsExistImage()) return "/images/no-product.png";
            return url;
        }
        public static string ToLogoImage(this string url, string baseUrl)
        {
            var value = baseUrl + url;

            if (!value.IsExistImage()) return "/images/no-logo.png";
            return value;
        }
        public static bool IsExistImage(this string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "HEAD";

            bool exists;
            try
            {
                request.GetResponse();
                exists = true;
            }
            catch
            {
                exists = false;
            }
            return exists;
        }
        public static string ToCurrency(this object value, string culture = "vi-VN", string prefix = "₫")
        {
            var data = value.ToSafetyString();
            if (data.IsNullOrEmpty()) return 0 + prefix;
            CultureInfo cul = CultureInfo.GetCultureInfo(culture);   // try with "en-US"
            string result = double.Parse(data).ToString("#,###", cul.NumberFormat);
            return result + prefix;
        }
        public static double ToSale(this object value, int sale)
        {
            var data = value.ToDouble();
            var percentage = (100 - sale) / 100;
            var price = data * percentage;
            return price;
        }
        public static string UrlEncode(this string value)
        {
            string result = value.ToSafetyString().ToLower();
            result = StripUnicodeCharactersFromString(result);
            string symbols = @"/\?#$& ";
            string[] charsToRemove = new string[] { "[", "]" };
            foreach (var c in charsToRemove)
            {
                result = result.Replace(c, string.Empty);
            }

            foreach (var item in symbols)
            {
                result = result.Replace(item, '-');
            }

            string pattern = "-+";
            Regex regex = new Regex(pattern);
            result = regex.Replace(result, "-");

            return result;
        }

        public static string GetFirstImage(this string imageList)
        {
            //Bắt lỗi null
            if (imageList == null || imageList.Trim() == string.Empty)
                return string.Empty;

            string result = imageList.Split("\n")[0].Trim();
            return result;
        }

        public static string[] SplitImages(this string imageList)
        {
            if (imageList == null)
                return new string[0];

            return imageList.Trim().Split('\n');
        }
      
    }
}
