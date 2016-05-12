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
using Mooshak26.Models.ViewModels;

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
        public ActionResult mySolutions(int id)
        {
            return View(_service.mySolutions(id));
        }

        // GET: Milestones/Details/5
        public ActionResult Details(int id)
        {
            var milestone1 = _service.GetMilestoneDetails(id);
            MilestoneViewModel temp = new MilestoneViewModel
            {
                milestone = milestone1,
                file = null
            };
            return View(temp);
        }

        [HttpPost]
       // public ActionResult Details(HttpPostedFileBase file)
        public ActionResult Details(MilestoneViewModel milestoneAndFile)
        {
            var userID = _service.GetUserID();
            if (milestoneAndFile.file.ContentLength > 0)
            {
                var FileExtension = Path.GetExtension(milestoneAndFile.file.FileName);
                var fileName = "program" + FileExtension;
                var path = Path.Combine(Server.MapPath("~/App_Data/Submissions"));
                var fullPath = Path.Combine(Server.MapPath("~/App_Data/Submissions"), fileName);

                milestoneAndFile.file.SaveAs(fullPath);
                if(FileExtension == ".py")
                {
                    _service.RunPythonProgram(milestoneAndFile.milestone, path);
                }
                else if(FileExtension == ".cpp")
                {
                    _service.RunCPlusPlusProgram(milestoneAndFile.milestone, path);
                }
               
            }
           
            return RedirectToAction("Feedback", new { id = milestoneAndFile.milestone.id });
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

        public ActionResult ViewSolutionsOf(int id)
        {
            var result = _us.getResults(_service.GetMilestoneDetails(id));
            return View(result);
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
