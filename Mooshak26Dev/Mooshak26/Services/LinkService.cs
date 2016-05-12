using Mooshak26.Models;
using Mooshak26.Models.Entities;
using Mooshak26.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooshak26.Services
{
    public class LinkService
    {
        private ApplicationDbContext _db;
        private CourseService _CourseService;
        private UserService _UserService;
        //For unit tests
        private readonly IAppDataContext _mockDB;
        public LinkService(IAppDataContext context)
        {
            _mockDB = context ?? new ApplicationDbContextTest();
        }

        public LinkService()
        {
            _db = new ApplicationDbContext();
            _CourseService = new CourseService();
            _UserService = new UserService();
        }
        //Gets Links to Courses...
        public List<Link> GetCoursesLinks(int courseID)
        {
            var list = _db.Links
               .Where(x => x.courseID == courseID).ToList();
            return list;
        }
        //Gets all links for Admin
        public List<Link> GetLinks()
        {
            return _db.Links.ToList();
        }
        //unused...
        public Link GetLinkDetails(int? id)
        {
            return _db.Links.Find(id);
        }
        //Creates links between course and user
        public Boolean CreateLink(Link newLink)
        {
            newLink.role = _UserService.GetRole(newLink.userID);
            var user = _UserService.GetUserDetails(newLink.userID);
            newLink.userName = user.userName;
            var course = _CourseService.CourseDetails(newLink.courseID);
            newLink.courseName = course.title;
            _db.Links.Add(newLink);
            _db.SaveChanges();
            return true;
        }

        public Boolean EditLink(Link link)
        {
            _db.Entry(link).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
        public Boolean DeleteLink(int id)
        {
            Link link = GetLinkDetails(id);
            _db.Links.Remove(link);
            _db.SaveChanges();
            return true;
        }
        // Returns the Id for every link the user is connected to...
        public List<int> UserLinks(int userID)
        {
            var list = _db.Links
                .Where(x => x.userID == userID)
                .Select(x => x.id).ToList();
            return list;
        }
        //This is to help Admin connnect users to courses..
        //and select a course from a dropdownlist
        public SelectList GetAllCourseTitles()
        {
            var list = new SelectList(
                _db.courses.ToList(),"id", "title");

                return list;
        }
        //This is to help Admin connnect users to courses..
        // and select a username from a dropdownlist...
        public SelectList GetAllUsernames()
        {
            var list = new SelectList(
                _db.MyUsers
                .Where( x => !x.role.Contains("Admin"))
                .ToList(), "id", "userName");
            return list;
        }
        ///
        public List<int> TestUserLinks(int userID)
        {
            var list = _mockDB.Links
                .Where(x => x.userID == userID)
                .Select(x => x.id).ToList();
            return list;
        }
        public List<Link> TestGetLinks()
        {
            return _mockDB.Links.ToList();
        }
    }
}