﻿using Mooshak26.Models;
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
    

        public CourseService()
        {
            _db = new ApplicationDbContext();
            _userService = new UserService();
        }

        public List<Course> GetCourses()
        {
            return _db.courses.ToList();
        }

        public int GetUserID()
        {
            var userName = _userService.GetUserName();
            var userID = _db.MyUsers.SingleOrDefault(x => x.userName == userName).id;
            return userID;
        }
        

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
                var course =_db.courses.SingleOrDefault(x => x.id == i);
                courseList.Add(course);
            }
            return courseList;
        }
        

        public Course CourseDetails(int? id)
        {
            return _db.courses.Find(id);
        }

        public int FindCourseIDByName(string courseName)
        {
            var courseID = _db.courses
                .Where(x => x.title == courseName)
                .Select(i => i.id).Single();
            return courseID;
        }

        public bool CreateCourse(Course course)
        {
            _db.courses.Add(course);
            _db.SaveChanges();
            return true;
        }
        
        public bool EditCourse(Course course)
        {
            _db.Entry(course).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
        
        public bool DeleteCourse(Course course)
        {
            _db.courses.Remove(course);
            _db.SaveChanges();
            return true;
        }
    }
}