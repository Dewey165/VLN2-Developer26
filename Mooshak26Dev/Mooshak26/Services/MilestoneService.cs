using Mooshak26.Models;
using Mooshak26.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooshak26.Services
{
    public class MilestoneService
    {
        private ApplicationDbContext _db;
        private AssignmentService _assignmentService;

        public MilestoneService()
        {
            _db = new ApplicationDbContext();
        }
        public List<Milestone> GetMilestonesByAssignmentID(int assignmentID)
        {
            var milestones = _db.Milestones
                .Where(x => x.assignmentID == assignmentID).ToList();
            return milestones;
        }
        public Milestone GetMilestoneDetails(int id)
        {
            return _db.Milestones.Find(id);
        }
        public Boolean CreateMilestone(Milestone milestone)
        {
            _db.Milestones.Add(milestone);
            _db.SaveChanges();
            return true;
        }
        public int GetUserID()
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var userID = _db.MyUsers.SingleOrDefault
               (x => x.userName == userName).id;

            return userID;
        }
     
        public SelectList GetUsersAssignmentTitles(int assignmentID)
        {
            var userId = GetUserID();
            _assignmentService = new AssignmentService();

            var courseID = _db.Assignments
                .SingleOrDefault(x => x.id == assignmentID)
                .courseID;
            var courseAssignments = _db.Assignments
                .Where(x => x.courseID == courseID).ToList();
            var list = new SelectList(
                courseAssignments.ToList(), "id", "title");

            return list;


        }
        public Boolean EditMilestone(Milestone milestone)
        {
            _db.Entry(milestone).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
        public Boolean DeleteMilestone(int id)
        {
            Milestone milestone = GetMilestoneDetails(id);
            _db.Milestones.Remove(milestone);
            _db.SaveChanges();
            return true;
        }
    }
}