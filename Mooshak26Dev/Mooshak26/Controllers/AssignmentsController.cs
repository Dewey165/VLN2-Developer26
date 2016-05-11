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

namespace Mooshak26.Controllers
{
    public class AssignmentsController : Controller
    {
       
        private AssignmentService _service = new AssignmentService();

        // GET: Assignments
        public ActionResult Index(int id)
        {
            if (_service.GetRole() == "Teacher")
            {
                return RedirectToAction("TeachersIndex", new { id = id });
            }
            return View(_service.GetAssignmentsInCourse(id));
        }
        public ActionResult TeachersIndex(int id)
        {
            return View(_service.GetAssignmentsInCourse(id));
        }

        // GET: Assignments/Details/5
        
        public ActionResult Details(int id)
        {
            return View(_service.GetAssignmentDetails(id));
        }

        // GET: Assignments/Create
        
        public ActionResult Create()
        {
            ViewBag.Courses = _service.GetUsersCoursesTitles();
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "assignmentID,courseID,title,description,totalGrade,timeSubmitted,dueDate")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                if(_service.CreateAssignment(assignment))
                {
                    return RedirectToAction("Index", new { id = assignment.courseID });
                }
            }
            return View(assignment);
        }
        
        // GET: Assignments/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_service.GetAssignmentDetails(id));
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "assignmentID,courseID,title,description,totalGrade,timeSubmitted,dueDate")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                if(_service.EditAssignment(assignment))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        //[Authorize(Roles = "Teacher")]
        public ActionResult Delete(int id)
        {

            Assignment assignment = _service.GetAssignmentDetails(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
       // [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _service.DeleteAssignment(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               // db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
