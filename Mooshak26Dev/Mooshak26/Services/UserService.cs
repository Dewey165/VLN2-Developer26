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
using System.Net.Mail;
using System.Text;

namespace Mooshak26.Services
{
    public class UserService
    {
        private static Random rnd = new Random();
        private ApplicationDbContext _db;
        private DeleteService _deleteService;
        //For unit tests
        private readonly IAppDataContext _mockDB;
        public UserService(IAppDataContext context)
        {
            _mockDB = context ?? new ApplicationDbContextTest();
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
        //Sends a random password to the user just created...
        public void SendPasswordInEmail(string password, string receiver, string userName)
        {
            string from = "bobbyjenkinsforreal@gmail.com";
            string to = receiver;
            string subject = "Mooshak26 Account Creation";
            string body = "An account has been created for you on Mooshak26.\n" 
                + "\n\nYour username is: " + userName +
                "\n\n Password to login is: " + password;

            MailMessage emailMessage = new MailMessage(from, to, subject, body);

            // We use gmail to send, and therefore this information is necessary.
            SmtpClient emailClient = new SmtpClient("smtp.gmail.com", 587);
            emailClient.UseDefaultCredentials = true;
            emailClient.EnableSsl = true;

            // The email used to send the email and the password. 
            // This must be hard-coded to the dummy email.
            emailClient.Credentials = new System.Net.NetworkCredential("bobbyjenkinsforreal@gmail.com", "TheRealJenk");
            emailClient.Send(emailMessage);
        }
        //Generate Random Password...
        public string RandomPasswordGenerator()
        {
            const int numberOfCapitalLetters = 3;
            const int numberOfSmallLetters = 3;
            const int numberOfSpecialCharacters = 1;

            const string capitalLetters = "QWERTYUIOPASDFGHJKLZXCVBNM";
            const string smallLetters = "qwertyuiopasdfghjklzxcvbnm";
            const string digits = "012345689";
            const string specialChars = "$-+?_&=!%{}/";
            
            StringBuilder password = new StringBuilder();
            for (int i = 1; i <= numberOfCapitalLetters; i++)
            {
                char capLetter = GenerateChar(capitalLetters);
                InsertRandom(password, capLetter);
            }
            for (int i = 1; i <= numberOfSmallLetters; i++)
            {
                char smallLetter = GenerateChar(smallLetters);
                InsertRandom(password, smallLetter);
            }
            char digit = GenerateChar(digits);
            InsertRandom(password, digit);

            for (int i = 1; i <= numberOfSpecialCharacters; i++)
            {
                char specialChar = GenerateChar(specialChars);
                InsertRandom(password, specialChar);
            }
            return password.ToString();
        }
        private static void InsertRandom(StringBuilder password, char character)
        {
            
            int randomPos = rnd.Next(password.Length + 1);
            password.Insert(randomPos, character);
        }
        private static char GenerateChar(string usableChars)
        {
            Random rnd = new Random();
            int randomStuff = rnd.Next(usableChars.Length);
            char randomChar = usableChars[randomStuff];
            return randomChar;
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