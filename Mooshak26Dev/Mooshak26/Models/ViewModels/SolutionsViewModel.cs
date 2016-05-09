using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak26.Models.ViewModels
{
    public class SolutionsViewModel
    {
        public int Id { get; set; }
        public int courseID { get; set; }
        public int assignmentID { get; set; }
        public int milestoneID { get; set; }
        public string input { get; set; }
        public string output { get; set; }


    }
}