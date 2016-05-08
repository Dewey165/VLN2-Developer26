using Mooshak26.Models;
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
        

        public DeleteService()
        {
            _db = new ApplicationDbContext();
            _linkService = new LinkService();
        }
        public void DeleteLinks(int userID)
        {
            var linkList = _linkService.userLinks(userID);
            foreach (var i in linkList)
            {
                _linkService.DeleteLink(i);
            }
        }
    }
}