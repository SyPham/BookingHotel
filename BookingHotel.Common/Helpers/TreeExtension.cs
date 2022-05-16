using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingHotel.Model.Catalog;

namespace BookingHotel.Common.Helpers
{
    public static class TreeExtension
    {
        //public static IEnumerable<TreeFunctionModel> FlatToHierarchy(IEnumerable<TreeFunctionModel> list)
        //{
        //    // hashtable lookup that allows us to grab references to containers based on id
        //    var lookup = new Dictionary<string, TreeFunctionModel>();
        //    // actual nested collection to return
        //    var nested = new List<TreeFunctionModel>();

        //    foreach (TreeFunctionModel item in list)
        //    {
        //        if (lookup.ContainsKey(item.ParentId))
        //        {
        //            // add to the parent's child list
        //            lookup[item.ParentId].Childrens.Add(item);
        //        }
        //        else
        //        {
        //            // no parent added yet (or this is the first time)
        //            nested.Add(item);
        //        }
        //        lookup.Add(item.Id, item);
        //    }

        //    return nested;
        //}
        public static IEnumerable<TreeFunctionModel> FlatToHierarchy(IEnumerable<TreeFunctionModel> list, int parentId = 0)
        {
            return (from i in list
                    where i.ParentId == parentId
                    select new TreeFunctionModel
                    {
                        Id = i.Id,
                        ParentId = i.ParentId,
                        Name = i.Title,
                        Title = i.Title,
                        Url = i.Url,
                        // Icon = i.Icon,
                        Action = i.Action,
                        Controller = i.Controller,
                        ModuleName = i.ModuleName,
                        Childrens = (IList<TreeFunctionModel>)FlatToHierarchy(list, i.Id)
                    }).ToList();
        }
        public static IEnumerable<TreeFunctionModel> ToFlatToHierarchy(this IEnumerable<TreeFunctionModel> list)
        {
            return FlatToHierarchy(list);
        }
    }
}
