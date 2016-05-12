using Mooshak26.Models;
using Mooshak26.Models.Entities;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.Mvc;

namespace Mooshak26.Services
{
    public class SolutionService
    {
        private ApplicationDbContext _db;
        private AssignmentService assignService;
        private Solution solution;
        //For unit tests
        private readonly IAppDataContext _mockDB;

        public SolutionService(IAppDataContext context)
        {
            _mockDB = context ?? new ApplicationDbContext();
        }

        public SolutionService()
        {
            _db = new ApplicationDbContext();
            assignService = new AssignmentService();
            solution = new Solution();
        }

        public bool CreateSolution(Solution sol)
        {
            _db.Solutions.Add(sol);
            _db.SaveChanges();
            return true;
        }
        //Get info about milestone...
        public Solution GetSolutionCreationInfo(int milestoneID)
        {
            //getting milestone info...
            var milestone = _db.Milestones.Find(milestoneID);
            var assignment = _db.Assignments.Find(milestone.assignmentID);
            var newSolution = new Solution();
            newSolution.courseID = assignment.courseID;
            newSolution.assignmentID = milestone.assignmentID;
            newSolution.milestoneID = milestoneID;
            return newSolution;
        }
        //Get all solutions...
        public List<Solution> GetAllSolutions(int milestoneID)
        {
            return (_db.Solutions
                .Where
                (x => x.milestoneID == milestoneID).ToList());
        }

        public Boolean EditSolution(Solution solution)
        {
            _db.Entry(solution).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public Solution GetSolutionDetails(int id)
        {
                return (_db.Solutions.Find(id));
        }

        public bool DeleteSolution(Solution sol)
        {
            _db.Solutions.Remove(sol);
            _db.SaveChanges();
            return true;
        }
        //Functions for unit tests....
        public List<Solution> TestGetAllSolutions(int milestoneID)
        {
            return (_mockDB.Solutions
                .Where
                (x => x.milestoneID == milestoneID).ToList());
        }
    }

}