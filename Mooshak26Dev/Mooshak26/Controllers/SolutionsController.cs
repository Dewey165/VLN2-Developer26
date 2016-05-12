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
    public class SolutionsController : Controller
    {
        private SolutionService _service = new SolutionService();

        // GET: Solutions
        public ActionResult Index(int id)
        {
            return View(_service.GetAllSolutions(id));
        }

        // GET: Solutions/Details/5
        public ActionResult Details(int id)
        {
            return View(_service.GetSolutionDetails(id));
        }

        // GET: Solutions/Create
        public ActionResult Create(int id)
        {
            return View(_service.GetSolutionCreationInfo(id));
        }

        // POST: Solutions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,courseID,assignmentID,milestoneID,input,output")] Solution solution)
        {
            if (ModelState.IsValid)
            {
                if(_service.CreateSolution(solution))
                {
                    return RedirectToAction("Index", new { id = solution.milestoneID });
                }
            }

            return View(solution);
        }

        // GET: Solutions/Edit/5
        public ActionResult Edit(int id)
        { 
            return View(_service.GetSolutionDetails(id));
        }

        // POST: Solutions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,courseID,assignmentID,milestoneID,input,output")] Solution solution)
        {
            if (ModelState.IsValid)
            {
               if(_service.EditSolution(solution))
                {
                    return RedirectToAction("Index", new { id = solution.milestoneID });
                }
            }
            return View(solution);
        }

        public ActionResult Delete(int id)
        {
            Solution sol = _service.GetSolutionDetails(id);
            if (sol == null)
            {
                return HttpNotFound();
            }
            return View(sol);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Solution sol = _service.GetSolutionDetails(id);
            if (_service.DeleteSolution(sol))
            {
                return RedirectToAction("Index", new { id = sol.milestoneID });
            }
            //Here should be an error page..
            return HttpNotFound();
        }



    }
}
