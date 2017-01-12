using System;
using System.Collections.Generic;
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
                var routeValues = HttpContext.Current.Request.QueryString.ToDictionary();

                if (pageInfo.Page > 1)
                {
                    routeValues["page"] = pageInfo.Previous;
                    result = result.WithRouteLink(routeName, new RouteValueDictionary(routeValues), "previous", $"page:{pageInfo.Previous}");
                }

                if (pageInfo.Page < totalPages)
                {
                    routeValues["page"] = pageInfo.Next;
                    result = result.WithRouteLink(routeName, new RouteValueDictionary(routeValues), "next", $"page: {pageInfo.Next}");
                }
            }

            return result;
        }

        public static IDictionary<string, object> ToDictionary(this NameValueCollection col)
        {
            var values = new Dictionary<string, object>();
            foreach (string key in col.Cast<string>())
            {
                values[key] = col[key];
            }

            return values;
        }
    }
}