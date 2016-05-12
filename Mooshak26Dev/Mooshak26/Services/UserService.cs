using Mooshak26.Models;
using Mooshak26.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Web;

namespace Mooshak26.Services
{
    public class UserService
    {
        private ApplicationDbContext _db;
        private DeleteService _deleteService;
        //For unit tests
        private readonly IAppDataContext _mockDB;
        public UserService(IAppDataContext context)
        {
            _mockDB = context ?? new ApplicationDbContext();
        }

        public UserService()
        {
            _db = new ApplicationDbContext();
            
        }

        //Get all users
        public List<User> GetUsers()
        {
            return _db.MyUsers.ToList();
        }
        //Find current logged in userName
        public string GetUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }
        
        public User GetUserDetails(int? id)
        {
            return _db.MyUsers.Find(id);
        }
        //Get the Role of the User with userID
        public string GetRole(int id)
        {
            var userRole = _db.MyUsers.SingleOrDefault
                (x => x.id == id).role;
            return userRole;
        }
        //Finds the userID by userName
        public int FindUserIDByUsername(string username)
        {
            var userID = _db.MyUsers.SingleOrDefault
                (x => x.userName == username).id;
            return userID;
        }

        //Get the roles to create Teacher or Student.
        public SelectList GetRoles()
        {
            var list = new SelectList(
                _db.Roles.Where(
                    x => !x.Name.Contains("Admin"))
                    .ToList(), "Name", "Name");
            return list;
        }

        public Boolean CreateUser(User user)
        {
            _db.MyUsers.Add(user);
            _db.SaveChanges();
            return true;
        }

        public Boolean EditUser(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
        //Get the ApplicationUser by UserName..
        public ApplicationUser getUser(string userName)
        {
            return _db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public Boolean DeleteUser(int id)
        {
            //Deleting the user from our users table...
            User user = GetUserDetails(id);
            _db.MyUsers.Remove(user);
            //Deleting the user from the AspNet table...


            //Deleting every connection he has to the Link table..
            //Can't do it through LinkService since it has a connection to UserService...
            _deleteService = new DeleteService();
            _deleteService.DeleteLinks(id);
            
            //Finally save all the changes...
            _db.SaveChanges();
            return true;
        }
        //Functions for unit tests...
        public List<User> TestGetUsers()
        {
            return _mockDB.MyUsers.ToList();
        }

        public string TestGetRole(int id)
        {
            var userRole = _mockDB.MyUsers.SingleOrDefault
                (x => x.id == id).role;
            return userRole;
        }

        public int TestFindUserIDByUsername(string username)
        {
            var userID = _mockDB.MyUsers.SingleOrDefault
                (x => x.userName == username).id;
            return userID;
        }

    }
}