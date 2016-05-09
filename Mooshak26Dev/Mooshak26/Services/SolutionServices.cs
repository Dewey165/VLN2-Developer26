using Mooshak26.Models;
using Mooshak26.Models.Entities;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.Mvc;

namespace Mooshak26.Services
{
    public class SolutionServices
    {
        private ApplicationDbContext _db;
        private AssignmentService assignService;
        private Solution solution;
        public SolutionServices()
        {
            _db = new ApplicationDbContext();
            assignService = new AssignmentService();
            solution = new Solution();
        }
        public SolutionServices(int courseID, int assignID, int solID)
        {
            _db = new ApplicationDbContext();
            assignService = new AssignmentService();
            solution = new Solution();
            solution.assignmentID = assignID;
            solution.courseID = courseID;
            solution.Id = solID;
        }
        public Solution CreateSolution()
        {
            solution.input = SolutionInputs();
            solution.output = SolutionOutputs();
            AddSolToDB(solution);
            return solution;
        }
        public string SolutionInputs()
        {
            //show input pane done in the viewModel
            //receive the input from UI
            
            return null;
        }
        public string SolutionOutputs()
        {
            //show input pane
            //receive the input from UI

            return null;

        }
        private bool AddSolToDB(Solution sol)
        {
            if (_db.Solutions.Contains(sol))
            {
                return false;
            }
            _db.Solutions.Add(sol);
            _db.SaveChanges();
            return true;
        }
        public List<Solution> getAllSolutions(int milestoneId)
        {
            return (_db.Solutions
                .Where
                (x => x.milestoneID == milestoneId).ToList());
        }
        public Solution FindSol(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return (_db.Solutions.Find(id));
            }
        }
        public bool DeleteSolution(Solution sol)
        {
            _db.Solutions.Remove(sol);
            _db.SaveChanges();
            return true;
        }


    }

}