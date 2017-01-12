using ASP.NET.ApiAddits.RESTFul.Extensions;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
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

        //public IHttpActionResult Get()
        //{
        //    return Ok(db.Courses);
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

        #region - Pagging -
        //public IHttpActionResult Get(UInt16 page = 1, UInt16 pageSize = 5)
        //{
        //    var courses = db.Courses.OrderBy(c => c.CourseID);
        //    var count = db.Courses.Count();

        //    return Ok(courses.Skip(pageSize * (page - 1)).Take(pageSize))
        //        .AddPagination("DefaultApi", new PageInfo { Page = page, PageSize = pageSize, Count = count });
        //}
        #endregion

        public IHttpActionResult Get(UInt16 page = 1, UInt16 pageSize = 5, string orderby = "courseid", string where = null, string select = null)
        {
            var query = db.Courses.AsQueryable().ApplyWhere(where).ApplySort(orderby);
            var count = query.Count();

            return Ok(query.Skip(pageSize * (page - 1)).Take(pageSize).ApplySelect(select))
                .AddPagination("DefaultApi", new PageInfo { Page = page, PageSize = pageSize, Count = count });
        }
    }
}
