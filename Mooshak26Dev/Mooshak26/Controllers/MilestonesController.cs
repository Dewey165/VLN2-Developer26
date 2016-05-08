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
    public class MilestonesController : Controller
    {
        private MilestoneService _service = new MilestoneService();

        // GET: Milestones
        public ActionResult Index(int id)
        {
            return View(_service.GetMilestonesByAssignmentID(id));
        }

        // GET: Milestones/Details/5
        public ActionResult Details(int id)
        {
            return View(_service.GetMilestoneDetails(id));
        }

        // GET: Milestones/Create
        public ActionResult Create()
        {
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
                    return RedirectToAction("Index", new { id = milestone.assignmentID });
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
                    return RedirectToAction("Index");
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
            _service.DeleteMilestone(id);
            return RedirectToAction("Index");
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
