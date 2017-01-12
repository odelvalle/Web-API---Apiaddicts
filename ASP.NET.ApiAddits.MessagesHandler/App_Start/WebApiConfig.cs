using ASP.NET.ApiAddits.MessagesHandler.Handlers;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace ASP.NET.ApiAddits.MessagesHandler
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // List of delegating handlers.
            DelegatingHandler[] handlers = new DelegatingHandler[]
            {
                new ApiKeyHandler("123456")
            };

            // Create a message handler chain with an end-point.
            var routeHandlers = HttpClientFactory.CreatePipeline(
                new HttpControllerDispatcher(config), handlers);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "KeyApi",
                routeTemplate: "api2/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: routeHandlers
            );

            config.MessageHandlers.Add(new MethodOverrideHandler());
        }
    }
}
