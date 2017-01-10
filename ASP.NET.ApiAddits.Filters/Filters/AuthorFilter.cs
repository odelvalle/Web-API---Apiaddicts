using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ASP.NET.ApiAddits.Filters.Filters
{
    public class AuthorFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.Add("Author", "Omar del Valle Rodriguez");
        }
    }
}