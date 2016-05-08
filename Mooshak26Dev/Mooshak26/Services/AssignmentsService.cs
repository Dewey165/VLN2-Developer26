using Mooshak26.Models;
using Mooshak26.Models.Entities;
using Mooshak26.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooshak26.Services
{
    public class AssignmentService
    {
        private ApplicationDbContext _db;
        private DeleteService _deleteService;
        private CourseService _courseService;

        public AssignmentService()
        {
            _db = new ApplicationDbContext();
        }

        public List<Assignment> GetAssignmentsInCourse(int courseID)
        {
            var list = _db.Assignments
                .Where(x => x.courseID == courseID).ToList();
            return list;
        }
        public Assignment GetAssignmentDetails(int id)
        {
           return _db.Assignments.Find(id);
        }
        
        public Boolean CreateAssignment(Assignment assignment)
        {
            _db.Assignments.Add(assignment);
            _db.SaveChanges();
            return true;
        }
        public Boolean EditAssignment(Assignment assignment)
        {
            _db.Entry(assignment).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
        public Boolean DeleteAssignment(int id)
        {
            Assignment assignment = GetAssignmentDetails(id);
            _db.Assignments.Remove(assignment);
            _deleteService = new DeleteService();
            _deleteService.DeleteMilestones(id);
            _db.SaveChanges();
            return true;
        }
        public void RateAssignment(int assignmentID)
        {
            //ToDo 
        }
        public SelectList GetUsersCoursesTitles()
        {
            _courseService = new CourseService();
            var userID = _courseService.GetUserID();
            var userCourses = _courseService.GetCoursesByUserID(userID);
            var list = new SelectList(
             userCourses.ToList(), "id", "title");
 
            return list;
        }
        
        public AssignmentViewModel GetAssignmentByID(int assignmentID)
        {
            //TODO
            var assignment = _db.Assignments.SingleOrDefault(x => x.id == assignmentID);
            if (assignment == null)
            {
                // kasta villu eða null
            }

            var milestones = _db.Milestones
                .Where(x => x.assignmentID == assignmentID)
                .Select(x => new AssignmentViewModel
                {
                    title = x.title
                })
                .ToList();

            var viewModel = new AssignmentViewModel
            {
                title = assignment.title,
                Milestones = milestones
            };

            return viewModel;
        }

    }
}