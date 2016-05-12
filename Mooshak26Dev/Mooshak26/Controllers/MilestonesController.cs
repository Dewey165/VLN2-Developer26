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
using System.IO;

namespace Mooshak26.Controllers
{
    public class MilestonesController : Controller
    {
        private MilestoneService _service = new MilestoneService();
        private UserService _us = new UserService();
        private SolutionServices _ss = new SolutionServices();
        private Milestone milestoneCoding;
        // GET: Milestones
        public ActionResult Index(int id)
        {
            if (_service.GetRole() == "Teacher")
            {
                return RedirectToAction("TeachersIndex", new { id = id });
            }
            return View(_service.GetMilestonesByAssignmentID(id));
        }
        public ActionResult TeachersIndex(int id)
        {
            return View(_service.GetMilestonesByAssignmentID(id));
        }

        //ADD SOLUTION
        public ActionResult GoToSolution(int id)
        {
            if (_service.GetRole() == "Teacher")
            {
                return RedirectToAction("TeachersIndex");
            }
            User user = new Models.Entities.User();
            user.userName = _us.GetUserName();
            int uId = _us.FindUserIDByUsername(user.userName);
            string role = _us.GetRole(uId);
            if(role == "Teacher")
            {
                ViewBag.Solutions = _ss.GetAllSolutions(id);

                RedirectToAction("Create", "Solutions.Create");
            }
            return View();
        }

        // GET: Milestones/Details/5
        public ActionResult Details(int id)
        {
            milestoneCoding = _service.GetMilestoneDetails(id);
            return View(milestoneCoding);
        }

        [HttpPost]
        public ActionResult Details(HttpPostedFileBase file)
        {
            var userID = _service.GetUserID();
            if (file.ContentLength > 0)
            {
                var FileExtension = Path.GetExtension(file.FileName);
                var fileName = "program" + FileExtension;

                var path = Path.Combine(Server.MapPath("~/App_Data/Submissions"), fileName);

                file.SaveAs(path);
                if(FileExtension == ".py")
                {
                    _service.RunPythonProgram(milestoneCoding, path);
                }
                else if(FileExtension == ".cpp")
                {
                    _service.RunCPlusPlusProgram(milestoneCoding, path);
                }
               
            }
           
            return RedirectToAction("Feedback", new { id = userID });
        }
        public ActionResult Feedback(int id)
        {
            return View(_service.Feedback(id));
        }

        // GET: Milestones/Create
        public ActionResult Create(int assignmentID)
        {
            ViewBag.Assignments = _service.GetUsersAssignmentTitles(assignmentID);
            return View();
        }

        // POST: Milestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,assignmentID,title,description,grade")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                if(_service.CreateMilestone(milestone))
                {
                    return RedirectToAction("TeachersIndex", new { id = milestone.assignmentID });
                }
            }
            return View(milestone);
        }

        // GET: Milestones/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_service.GetMilestoneDetails(id));
        }

        // POST: Milestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,assignmentID,title,description,grade")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                if(_service.EditMilestone(milestone))
                {
                    return RedirectToAction("TeachersIndex", new { id = milestone.assignmentID });
                }
            }
            return View(milestone);
        }

        // GET: Milestones/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_service.GetMilestoneDetails(id));
        }

        // POST: Milestones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var assignmentID = _service.GetMilestoneDetails(id).assignmentID;
            _service.DeleteMilestone(id);
            return RedirectToAction("TeachersIndex", new { id = assignmentID });
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
