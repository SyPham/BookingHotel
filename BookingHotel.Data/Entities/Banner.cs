﻿using System;
using System.Collections.Generic;

#nullable disable

namespace BookingHotel.Data.Entities
{
    public partial class Banner
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public string Thumb { get; set; }
        public string Url { get; set; }
        public bool? Status { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public string BannerType { get; set; }
        public string SubTitle { get; set; }

        public virtual Account CreateByNavigation { get; set; }
    }
}
