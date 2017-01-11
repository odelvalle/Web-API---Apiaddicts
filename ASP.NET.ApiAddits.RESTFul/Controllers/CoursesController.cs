using ContosoUniversity.DAL;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using static ASP.NET.ApiAddits.RESTFul.Extensions.HttpActionResultExtensions;

namespace ASP.NET.ApiAddits.RESTFul.Controllers
{
    public class CoursesController : ApiController
    {
        private readonly SchoolContext db;

        public CoursesController()
        {
            this.db = new SchoolContext();
        }

        public IHttpActionResult Get(UInt16 page = 1, UInt16 pageSize = 5)
        {
            var courses = db.Courses;
            var count = db.Courses.Count();

            var response = Ok(courses.OrderBy(c => c.CourseID).Skip(pageSize * (page - 1)).Take(pageSize));
            return response.AddPagination("DefaultApi", new PageInfo { Page = page, PageSize = pageSize, Count = count });
        }

        //public IHttpActionResult Get(ODataQueryOptions<Course> options)
        //{
        //    options.Validate(WebApiConfig.ODataSettings);
        //    var settings = new ODataQuerySettings()
        //    {
        //        PageSize = 2
        //    };

        //    var courses = db.Courses.AsQueryable();
        //    var query = (IQueryable<Course>)options.ApplyTo(courses, settings);

        //    return Ok(new PageResult<Course>(query, Request.ODataProperties().NextLink, courses.Count()));
        //}

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

            if (student == null) return Ok(students);
            if (student.HasValue && !students.Any()) return StatusCode(System.Net.HttpStatusCode.NoContent);

            return Ok(students.First());
        }
    }
}
