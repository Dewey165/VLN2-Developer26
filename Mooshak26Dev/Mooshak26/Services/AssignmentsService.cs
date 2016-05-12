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
        //For unit tests
        private readonly IAppDataContext _mockDB;
        public AssignmentService(IAppDataContext context)
        {
            _mockDB = context ?? new ApplicationDbContextTest();
        }

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
        //Gets the role for the user for different views for teachers and students..
        public string GetRole()
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var userRole = _db.MyUsers.SingleOrDefault
              (x => x.userName == userName).role;
            return userRole;
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
        //This is left out because we didn't have time to implement it
        public void RateAssignment(int assignmentID)
        {
            //ToDo 
        }
        //Get all course titles that user is associated with...
        public SelectList GetUsersCoursesTitles()
        {
            _courseService = new CourseService();
            var userID = _courseService.GetUserID();
            var userCourses = _courseService.GetCoursesByUserID(userID);
            var list = new SelectList(
             userCourses.ToList(), "id", "title");
 
            return list;
        }
        public Assignment TestGetAssignmentDetails(int id)
        {
            return _mockDB.Assignments1.Find(id);
        }
    }
}