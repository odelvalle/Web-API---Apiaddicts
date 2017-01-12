using ASP.NET.ApiAddits.RESTFul.Extensions;
using ASP.NET.ApiAddits.RESTFul.Filters;
using ContosoUniversity.DAL;
using System;
using System.Linq;
using System.Web.Http;
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

        public IHttpActionResult Get()
        {
            return Ok(db.Courses);
        }

        #region - Pagging -

        [Route("api/v2.0/courses", Name = "Default20")]
        public IHttpActionResult Get(UInt16 page = 1, UInt16 pageSize = 3)
        {
            var courses = db.Courses.OrderBy(c => c.CourseID);
            var count = db.Courses.Count();

            return Ok(courses.Skip(pageSize * (page - 1)).Take(pageSize))
                .AddPagination("Default20", new PageInfo { Page = page, PageSize = pageSize, Count = count });
        }
        #endregion

        #region - Paging, OrderBY, Where and Select
        [CacheFilter(QueryStringParamsInCacheKey ="*")]
        [Route("api/v3.0/courses", Name = "Default30")]
        public IHttpActionResult Get(UInt16 page = 1, UInt16 pageSize = 3, string orderby = "courseid", string where = null, string select = null)
        {
            var query = db.Courses.AsQueryable().ApplyWhere(where).ApplySort(orderby);
            var count = query.Count();

            return Ok(query.Skip(pageSize * (page - 1)).Take(pageSize).ApplySelect(select))
                .AddPagination("Default30", new PageInfo { Page = page, PageSize = pageSize, Count = count });
        }
        #endregion

        [Route("api/courses/{id}")]
        [Route("api/v2.0/courses/{id}")]
        [Route("api/v3.0/courses/{id}")]
        public IHttpActionResult Get(int id)
        {
            var course = db.Courses.SingleOrDefault(c => c.CourseID == id);
            if (course == null) return NotFound();

            return Ok(course);
        }

        [HttpGet]
        [Route("api/courses/{id}/students/{student:int?}")]
        [Route("api/v2.0/courses/{id}/students/{student:int?}")]
        [Route("api/v3.0/courses/{id}/students/{student:int?}")]
        public IHttpActionResult StudentsInCourse(int id, int? student = null)
        {
            var students = db.Courses.Include("Enrollments.Student")
                .Where(c => c.CourseID == id)
                .SelectMany(c => c.Enrollments)
                .Where(e => student == null || e.StudentID == student).Select(e => e.Student);

            if (student == null) return Ok(students);
            if (!students.Any()) return StatusCode(System.Net.HttpStatusCode.NotFound);

            return Ok(students.First());
        }
    }
}
