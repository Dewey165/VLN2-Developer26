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
        public int GetCourseIDByAssignmentID(int assignmentID)
        {
            return _db.Assignments.SingleOrDefault(x => x.id == assignmentID).courseID;
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
        public List<SubmittedSolution> mySolutions(int milestoneID)
        {
            //We get in the milestoneID and look
            var userID = GetUserID();
            var submittedSolution =
                _db.SubmittedSolutions.Where(x => x.milestoneID == milestoneID)
                .Where(x => x.userID == userID).ToList();
            return submittedSolution;
        }
        public List<SubmittedSolution> Feedback(int milestoneID)
        {
            //We get in the milestoneID and look
            var userID = GetUserID();
            var count = _db.Solutions
                .Where(x => x.milestoneID == milestoneID).Count();

            var submittedSolution = 
                _db.SubmittedSolutions.Where(x => x.milestoneID == milestoneID)
                .Where(x => x.userID == userID).OrderByDescending(x => x.id).Take(count).ToList();
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
            //Get the inputs and correct outputs...
            List<string> inputs = GetInputs(milestone.id);
            var compilerFolder = "C:\\Program Files (x86)\\Microsoft Visual Studio 14.0\\VC\\bin\\";
            var cppFileName = "Program.cpp";
            // Execute the compiler:
            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = path;
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
            List<string> userOutputs = new List<string>();
            foreach(string i in inputs)
            {
                if (System.IO.File.Exists(path + "\\" + cppFileName))
                {
                    var processInfoExe = new ProcessStartInfo(path + "\\Program.exe", "");
                    processInfoExe.UseShellExecute = false;
                    processInfoExe.RedirectStandardOutput = true;
                    processInfoExe.RedirectStandardError = true;
                    processInfoExe.RedirectStandardInput = true;
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
                        processExe.StandardInput.WriteLine(i);
                        // We then read the output of the program and add it to userOutputs...
                        while (!processExe.StandardOutput.EndOfStream)
                        {
                            userOutputs.Add(processExe.StandardOutput.ReadLine());
                        }
                    }
                }
            }
            saveSubmittedSolution(milestone, userOutputs);
        }
        public void saveSubmittedSolution(Milestone milestone, List<string> userOutputs)
        {
            int courseID = GetCourseIDByAssignmentID(milestone.assignmentID);
            int assignmentID = milestone.assignmentID;
            int milestoneID = milestone.id;
            int userID = GetUserID();
            //Get the inputs and outputs the teacher set up...
            List<string> inputs = GetInputs(milestone.id);
            List<string> correctOutputs = GetOutputs(milestone.id);
            for (int i = 0; i < inputs.Count; i++)
            {
                bool correct = false;
                if (userOutputs[i] == correctOutputs[i])
                {
                    correct = true;
                }
                var result = new SubmittedSolution
                {
                    courseID = courseID,
                    assignmentID = assignmentID,
                    milestoneID = milestoneID,
                    userID = userID,
                    output = userOutputs[i],
                    expectedOutput = correctOutputs[i],
                    input = inputs[i],
                    correct = correct
                };
                _db.SubmittedSolutions.Add(result);
                _db.SaveChanges();
            }
        }
    }
}