using ContosoUniversity.DAL;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;

namespace ASP.NET.ApiAddits.RESTFul.Controllers
{
    public class CoursesController : ApiController
    {
        private readonly SchoolContext db;

        public CoursesController()
        {
            this.db = new SchoolContext();
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(db.Courses.AsQueryable());
        }

        public IHttpActionResult Get(int id)
        {
            var course = db.Courses.SingleOrDefault(c => c.CourseID == id);
            if (course == null) return NotFound();

            return Ok(course);
        }

        [HttpGet]
        [Route("api/courses/{id}/students/{student:int?}")]
        public IHttpActionResult StudentsInCourse(int id, int? student = null)
        {
            var students = db.Courses.Include("Enrollments.Student")
                .Where(c => c.CourseID == id)
                .SelectMany(c => c.Enrollments)
                .Where(e => student == null || e.StudentID == student).Select(e => e.Student);

            if (students == null) return Ok(students);
            if (student != null && !students.Any()) StatusCode(System.Net.HttpStatusCode.NoContent);

            return Ok(students.First());
        }
    }
}
