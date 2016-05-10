using Mooshak26.Models;
using Mooshak26.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak26.Services
{
    public class DeleteService
    {
        private ApplicationDbContext _db;
        private LinkService _linkService;
        private MilestoneService _milestoneService;

        public DeleteService()
        {
            _db = new ApplicationDbContext();
            _linkService = new LinkService();
            _milestoneService = new MilestoneService();
        }
        public void DeleteLinks(int userID)
        {
            var linkList = _linkService.userLinks(userID);
            foreach (var i in linkList)
            {
                _linkService.DeleteLink(i);
            }
        }
        public void DeleteMilestones(int assignmentID)
        {
            _milestoneService = new MilestoneService();
            var milestones = _milestoneService.GetMilestonesByAssignmentID(assignmentID);
            foreach (var i in milestones)
            {
                _milestoneService.DeleteMilestone(i.id);
            }
        }
        public void DeleteCourse(int id)
        {
            var linkCourses = _linkService.GetCoursesLinks(id);
            foreach(Link item in linkCourses)
            {
                _db.Links.Remove(item);
            }
            //Todo delete assignments, milestones, solutions, submittedSolutions, 

        }
    }
}