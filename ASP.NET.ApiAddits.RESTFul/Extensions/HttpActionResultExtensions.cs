﻿using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebApi.LinkHeader;

namespace ASP.NET.ApiAddits.RESTFul.Extensions
{
    public static class HttpActionResultExtensions
    {
        public class PageInfo
        {
            public ushort Page { get; set; }
            public ushort PageSize { get; set; }
            public int Count { get; set; }

            public ushort Previous => (ushort)(Page - 1);
            public ushort Next => (ushort)(Page + 1);
        }

        public static IHttpActionResult AddPagination(this IHttpActionResult result, string routeName, PageInfo pageInfo)
        {
            var totalPages = (int)Math.Ceiling((double)pageInfo.Count / pageInfo.PageSize);
            if (totalPages > 1)
            {
                var routeValues = HttpContext.Current.Request.QueryString.ToRouteValues();

                if (pageInfo.Page > 1)
                {
                    result = result.WithRouteLink(routeName, routeValues, "previous", $"page:{pageInfo.Previous}");
                }

                if (pageInfo.Page < totalPages)
                {
                    result = result.WithRouteLink(routeName, routeValues, "next", $"page: {pageInfo.Next}");
                }
            }

            return result;
        }

        public static RouteValueDictionary ToRouteValues(this NameValueCollection col)
        {
            var values = new RouteValueDictionary();
            foreach (string key in col.Cast<string>())
            {
                values[key] = col[key];
            }

            return values;
        }
    }
}