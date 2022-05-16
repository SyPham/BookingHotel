
using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace BookingHotel.Common.Helpers
{
    public static class HttpExtensions
    {
        public static void AddFileNameHeader(this HttpResponse response, string fileName
            )
        {
            response.Headers.Add("name", fileName);

        }
    }
}