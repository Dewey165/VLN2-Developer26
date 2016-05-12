using Mooshak26.Models;
using Mooshak26.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IronPython.Hosting;

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

        public string GetRole()
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var userRole = _db.MyUsers.SingleOrDefault
              (x => x.userName == userName).role;
            return userRole;
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
        public List<SubmittedSolution> Feedback(int id)
        {
            var submittedSolution = 
                _db.SubmittedSolutions.Where(x => x.userID == id).ToList();
            return submittedSolution;
        }
        public List<string> GetInputs(int milestoneID)
        {
            return _db.Solutions
                .Where(x => x.milestoneID == milestoneID)
                .Select(x => x.input).ToList();
        }
        public List<string> GetOutputs(int milestoneID)
        {
            return _db.Solutions
                .Where(x => x.milestoneID == milestoneID)
                .Select(x => x.output).ToList();
        }
        public void RunPythonProgram(Milestone milestone, string path)
        {
        
        }
        /*
        public void RunPythonProgram(Milestone milestone, string path)
        {
            string pythonInterpreter = @"C:\Users\GodDewey\Anaconda3\python.exe";
            
            int x = 2;
            int y = 5;
            ProcessStartInfo start = new ProcessStartInfo(pythonInterpreter);
            // make sure we can read the output from stdout 
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            // List<string> inputs = GetInputs(milestone.id);
            //   List<string> outputs = GetInputs(milestone.id);
            // start python app with 3 arguments  
            // 1st argument is pointer to itself, 2nd and 3rd are actual arguments we want to send 
            start.Arguments = string.Format("{0} {1} {2}",path, x, y);
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
            
        }   
        */
        public void RunCPlusPlusProgram(Milestone milestone, string path)
        {
            int x = 2;
            int y = 5;
            var workingFolder = "C:\\Users\\GodDewey\\Desktop\\VLN2\\dev\\VLN2-Developer26\\Mooshak26Dev\\Mooshak26\\App_Data\\Submissions\\";
           

            var compilerFolder = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\bin\\";
            var cppFileName = "Program.cpp";
            // Execute the compiler:
            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;

            compiler.Start();
            compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
            compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + cppFileName);
            compiler.StandardInput.WriteLine("exit");
            string output = compiler.StandardOutput.ReadToEnd();
            compiler.WaitForExit();
            compiler.Close();

            // Check if the compile succeeded, and if it did,
            // we try to execute the code:
            if (System.IO.File.Exists(path))
            {
                var processInfoExe = new ProcessStartInfo(path, x + " " + y);
                processInfoExe.UseShellExecute = false;
                processInfoExe.RedirectStandardOutput = true;
                processInfoExe.RedirectStandardError = true;
                processInfoExe.CreateNoWindow = true;
                using (var processExe = new Process())
                {
                    processExe.StartInfo = processInfoExe;
                    processExe.Start();
                    // In this example, we don't try to pass any input
                    // to the program, but that is of course also
                    // necessary. We would do that here, using
                    // processExe.StandardInput.WriteLine(), similar
                    // to above.

                    // We then read the output of the program:
                    var lines = new List<string>();
                    while (!processExe.StandardOutput.EndOfStream)
                    {
                        lines.Add(processExe.StandardOutput.ReadLine());
                    }

                    
                }
            }
        }
    }
}