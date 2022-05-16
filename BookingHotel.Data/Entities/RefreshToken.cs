using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace BookingHotel.Data.Entities
{
    //[Owned]
    public class RefreshToken
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? CreateTime { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? RevokeTime { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => RevokeTime == null && !IsExpired;

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
