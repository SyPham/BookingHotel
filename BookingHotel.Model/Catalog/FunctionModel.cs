using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookingHotel.Model.Catalog
{
    public class FunctionModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public int? Position { get; set; }
        public bool Status { get; set; }
        public string Icon { get; set; }
        public bool? IsShow { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModuleId { get; set; }
        public int? ParentId { get; set; }
        public string ModuleName { get; set; }

    }
    public class TreeFunctionModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Note { get; set; }
        public string ModuleName { get; set; }
        public int? Position { get; set; }
        public bool Status { get; set; }
        public bool? IsShow { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? ModuleId { get; set; }
        public int? ParentId { get; set; }
        public int? Sequence { get; set; }
        public bool HasChildren
        {
            get { return Childrens.Any(); }
        }
        public IList<TreeFunctionModel> Childrens { get; set; } = new List<TreeFunctionModel>();
    }
}
