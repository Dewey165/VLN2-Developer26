using Mooshak26.Models;
using Mooshak26.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mooshak26.Services
{
    public class MilestoneService
    {
        private ApplicationDbContext _db;

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