using Mooshak26.Models;
using Mooshak26.Models.Entities;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.Mvc;

namespace Mooshak26.Services
{
    public class CourseService
    {
        private ApplicationDbContext _db;
        private UserService _userService;
        private DeleteService _deleteService;
        //For unit tests
        private readonly IAppDataContext _mockDB;
        public CourseService(IAppDataContext context)
        {
            _mockDB = context ?? new ApplicationDbContextTest();
        }

        public CourseService()
        {
            _db = new ApplicationDbContext();
            _userService = new UserService();
        }
      
        //Returns Admin all Courses in Database.
        public List<Course> GetCourses()
        {

            return _db.courses.ToList();
        }
        //Gets the UserID from logged in user and user table...
        public int GetUserID()
        {
            var userName = _userService.GetUserName();
            if (userName == "")
            {
                return 0;
            }
            var userID = _db.MyUsers.SingleOrDefault(x => x.userName == userName).id;

            return userID;
        }
        //Gets user role through UserID...
        public string GetUserRole()
        {
            var userID = GetUserID();
            return _userService.GetRole(userID);
        }
        //Get courses user is registerred in, if Admin then return all courses..
        public List<Course> GetCoursesByUserID(int id)
        {
            if (_userService.GetRole(id) == "Admin")
            {
                return GetCourses();
            }

            var list = _db.Links
                .Where(x => x.userID == id)
                .Select(x => x.courseID).ToList();
            var courseList = new List<Course>();
            foreach (int i in list)
            {
                var course = _db.courses.SingleOrDefault(x => x.id == i);
                courseList.Add(course);
            }
            return courseList;
        }
        //Get specific Course by it's id for Details, Edit, Delete...
        public Course CourseDetails(int? id)
        {
            return _db.courses.Find(id);
        }
   
        //Find the courseID by name of the course...
        public int FindCourseIDByName(string courseName)
        {
            var courseID = _db.courses
                .Where(x => x.title == courseName)
                .Select(i => i.id).Single();
            return courseID;
        }
        //Create function to add the course to the database..
        public bool CreateCourse(Course course)
        {
            _db.courses.Add(course);
            _db.SaveChanges();
            return true;
        }
        //Function to edit the course and change the data in db...
        public bool EditCourse(Course course)
        {
            _db.Entry(course).State = EntityState.Modified;
            //Todo change the link db...
            _db.SaveChanges();
            return true;
        }
        //DeleteService helps to delete all assignments
        //and milestones and solutions associated to it...
        public bool DeleteCourse(Course course)
        {
            _deleteService = new DeleteService();
            _deleteService.DeleteCourse(course.id);
            _db.courses.Remove(course);
           
            _db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Test functions to talk to _mockDB
        /// </summary>
        /// <returns></returns>
        //This is just to talk to the mock database for unit tests...
        public List<Course> TestGetCourses()
        {

            return _mockDB.courses.ToList();
        }
        //Find the courseID by name of the course...
        public int TestFindCourseIDByName(string courseName)
        {
            var courseID = _mockDB.courses
                .Where(x => x.title == courseName)
                .Select(i => i.id).Single();
            return courseID;
        }
    }
}