using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System.Web.Http;

namespace ASP.NET.ApiAddits.MessagesHandler.Controllers
{
    public class StudentsController : ApiController
    {
        public IHttpActionResult Post([FromBody]Student student)
        {
            return InternalServerError();
        }

        public IHttpActionResult Put(int id, [FromBody]Student student)
        {
            student.ID = id;
            return Ok(student);
        }
    }
}
