using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mooshak26.Models.Entities;

namespace Mooshak26.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
       // public virtual User MyUsers { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IAppDataContext
    {
        IDbSet<Assignment> Assignments1 { get; set; }
        IDbSet<Course> courses { get; set; }
        IDbSet<Link> Links { get; set; }
        IDbSet<Milestone> Milestones { get; set; }
        IDbSet<User> MyUsers { get; set; }
        IDbSet<Solution> Solutions { get; set; }
        IDbSet<SubmittedSolution> SubmittedSolutions { get; set; }

        //user
        //link
        //milestone
        //assignments
        int SaveChanges();
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Solution> Solutions { get; set;}
        public DbSet<Course> courses { get; set; }
        public DbSet<User> MyUsers { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<SubmittedSolution> SubmittedSolutions { get; set; }
        //For the unit tests       

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
    public class ApplicationDbContextTest : IdentityDbContext<ApplicationUser>, IAppDataContext
    {
        //For the unit tests
        IDbSet<Assignment> IAppDataContext.Assignments1 { get; set; }
        IDbSet<Milestone> IAppDataContext.Milestones { get; set; }
        IDbSet<Solution> IAppDataContext.Solutions { get; set; }
        IDbSet<Course> IAppDataContext.courses { get; set; }
        IDbSet<User> IAppDataContext.MyUsers { get; set; }
        IDbSet<Link> IAppDataContext.Links { get; set; }
        IDbSet<SubmittedSolution> IAppDataContext.SubmittedSolutions { get; set; }


        public ApplicationDbContextTest()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}