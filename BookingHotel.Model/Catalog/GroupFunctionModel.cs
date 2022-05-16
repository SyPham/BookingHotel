using System;
using System.Collections.Generic;
using System.Text;
using BookingHotel.Data.Entities;

namespace BookingHotel.Model.Catalog
{
    public class GroupFunctionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public bool? IsDelete { get; set; }
    }


    public class PostGroupFunctionRequest
    {
        public GroupFunctionModel GroupFunction { get; set; }
        public List<int> FunctionIds { get; set; } = new List<int>();
    }
    public class PutGroupFunctionRequest
    {
        public GroupFunctionModel GroupFunction { get; set; }
        public List<int> FunctionIds { get; set; } = new List<int>();
    }
}
