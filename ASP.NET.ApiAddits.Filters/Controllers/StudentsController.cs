using ASP.NET.ApiAddits.Filters.Filters;
using System.Web.Http;

namespace ASP.NET.ApiAddits.Filters.Controllers
{
    public class StudentsController : ApiController
    {
        //[AuthorFilter]
        public IHttpActionResult Get()
        {
            return Ok();
        }
        public IHttpActionResult Get(int id)
        {
            return Ok();
        }
    }
}
