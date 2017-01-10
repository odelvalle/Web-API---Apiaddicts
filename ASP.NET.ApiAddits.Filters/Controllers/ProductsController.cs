using ASP.NET.ApiAddits.Filters.Filters;
using ASP.NET.ApiAddits.Filters.Models;
using System;
using System.Web.Http;

namespace ASP.NET.ApiAddits.Filters.Controllers
{
    public class ProductsController : ApiController
    {
        [ValidateFilter]
        public IHttpActionResult Post([FromBody]Product product)
        {
            product.Id = new Random().Next();
            return Created(new Uri(Url.Link("DefaultApi", new { id = product.Id })), product);
        }

        [AuthenticationFilter]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
