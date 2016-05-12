using Mooshak26.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak26.Models.ViewModels
{
    public class MilestoneViewModel
    {
        public Milestone milestone { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}