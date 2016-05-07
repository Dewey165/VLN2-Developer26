using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak26.Models.Entities
{
    public class Link
    {
        public int id { get; set; }
        public int userID { get; set; }
        public string userName { get; set; }
        public int courseID { get; set; }
        public string courseName { get; set; }
        public string role { get; set; }
    }
}