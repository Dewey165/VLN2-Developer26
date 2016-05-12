using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mooshak26.Models;
using System.Data.Entity;
using Mooshak26.Models.Entities;
using Mooshak26.Tests.SozialWeb.Tests;

namespace Mooshak26.Tests
{
    class MockDataContext : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDataContext()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            courses = new InMemoryDbSet<Course>();
            Assignments1 = new InMemoryDbSet<Assignment>();
            Solutions = new InMemoryDbSet<Solution>();
            MyUsers = new InMemoryDbSet<User>();
            Links = new InMemoryDbSet<Link>();
            SubmittedSolutions = new InMemoryDbSet<SubmittedSolution>();
            Milestones = new InMemoryDbSet<Milestone>();
        }

        public IDbSet<Course> courses { get; set; }
        public IDbSet<Milestone> Milestones { get; set; }
        public IDbSet<Assignment> Assignments1 { get;  set; }
        public IDbSet<Solution> Solutions { get;  set; }
        public IDbSet<User> MyUsers { get;  set; }
        public IDbSet<Link> Links { get;  set; }
        public IDbSet<SubmittedSolution> SubmittedSolutions { get; set; }

        // TODO: bætið við fleiri færslum hér
        // eftir því sem þeim fjölgar í AppDataContext klasanum ykkar!

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}
