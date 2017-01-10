using ASP.NET.ApiAddits.Filters.Filters;
using System.Web.Http;

namespace ASP.NET.ApiAddits.Filters
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Filters
            config.Filters.Add(new AuthorFilter());
        }
    }
}
