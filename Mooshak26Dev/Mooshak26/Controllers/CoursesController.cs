using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mooshak26.Models;
using Mooshak26.Models.Entities;
using Mooshak26.Services;
using System.Web.UI.WebControls;

namespace Mooshak26.Controllers
{
    public class CoursesController : Controller
    {
        // private ApplicationDbContext db = new ApplicationDbContext();
        private CourseService _service = new CourseService();
       // private AssignmentsService _assignmentService = new AssignmentsService();

        // GET: Courses
        public ActionResult Index()
        {
         
            var userID = _service.GetUserID();
            if (userID == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            if(_service.GetUserRole() == "Teacher")
            {
                return RedirectToAction("TeacherMainPage");
            }
            else if (_service.GetUserRole() == "Student")
            {
                return RedirectToAction("StudentMainPage");
            }
            else
            {
                return RedirectToAction("AdminMainPage", "Home");
            }       
        }
   
        public ActionResult AdminCourses()
        {
            var userID = _service.GetUserID();
            return View(_service.GetCoursesByUserID(userID));
        }
        public ActionResult TeacherMainPage()
        {
            var userID = _service.GetUserID();
            return View(_service.GetCoursesByUserID(userID));
        }
        public ActionResult StudentMainPage()
        {
            var userID = _service.GetUserID();
            return View(_service.GetCoursesByUserID(userID));
        }

        // GET: Courses/Details/5
        public ActionResult Details(int id)
        {
            
            //Get course details through the CourseService...
            Course course = _service.CourseDetails(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
            /* 
            //Get assignments for each course in Course Details..
            var assignments = _assignmentService.GetAssignmentsInCourse(id);
            return View(assignments);
            */

        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,description")] Course course)
        {
            if (ModelState.IsValid)
            {
                if (_service.CreateCourse(course))
                {
                    return RedirectToAction("AdminCourses");
                }
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _service.CourseDetails(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,description")] Course course)
        {
            if (ModelState.IsValid)
            {
                if (_service.EditCourse(course))
                {
                    return RedirectToAction("AdminCourses");
                }
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = _service.CourseDetails(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = _service.CourseDetails(id);
            if (_service.DeleteCourse(course))
            {
                    return RedirectToAction("AdminCourses");   
            }
            //Here should be an error page..
            return HttpNotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
