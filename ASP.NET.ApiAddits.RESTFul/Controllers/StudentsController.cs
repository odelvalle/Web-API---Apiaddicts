using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using Marvin.JsonPatch;
using System;
using System.Linq;
using System.Web.Http;

namespace ASP.NET.ApiAddits.RESTFul.Controllers
{
    public class StudentsController : ApiController
    {
        private readonly SchoolContext db;

        public StudentsController()
        {
            this.db = new SchoolContext();
        }

        public IHttpActionResult Get()
        {
            return Ok(db.Students);
        }

        public IHttpActionResult Get(int id)
        {
            var student = db.Students.SingleOrDefault(s => s.ID == id);
            if (student == null) return NotFound();

            return Ok(student);
        }

        public IHttpActionResult Post(Student student)
        {
            db.Students.Add(student);
            db.SaveChanges();

            return Created(new Uri(Url.Link("DefaultApi", new { id = student.ID })), student);
        }

        public IHttpActionResult Put(int id, Student student)
        {
            if (!db.Students.Any(s => s.ID == id)) return NotFound();

            student.ID = id;
            var result = db.Entry(student).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

            return Ok(student);
        }

        public IHttpActionResult Patch(int id, [FromBody]JsonPatchDocument<Student> studentPatchDocument)
        {
            var student = db.Students.SingleOrDefault(s => s.ID == id);
            if (student == null) return NotFound();

            studentPatchDocument.ApplyTo(student);
            db.SaveChanges();

            return Ok(student);
        }

        public IHttpActionResult Delete(int id)
        {
            var student = db.Students.SingleOrDefault(s => s.ID == id);
            if (student == null) return NotFound();

            db.Students.Remove(student);
            db.SaveChanges();

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
