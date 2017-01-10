using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}
