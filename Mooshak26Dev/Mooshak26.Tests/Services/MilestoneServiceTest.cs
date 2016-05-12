using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mooshak26.Models.Entities;
using Mooshak26.Services;

namespace Mooshak26.Tests.Services
{
    [TestClass]
    public class MilestoneServiceTest
    {
        private MilestoneService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDB = new MockDataContext();
            _service = new MilestoneService();
            var a1 = new Milestone
            {
                id = 1,
                assignmentID = 1,
                title = "test1",
                description = "test1",
                grade = 1
               
            };
            mockDB.Milestones.Add(a1);
            var a2 = new Milestone
            {
                id = 2,
                assignmentID = 2,
                title = "test2",
                description = "test2",
                grade = 2
            };
            mockDB.Milestones.Add(a2);

            var a3 = new Milestone
            {
                id = 3,
                assignmentID = 3,
                title = "test3",
                description = "test3",
                grade = 3
            };
            mockDB.Milestones.Add(a3);
            _service = new MilestoneService(mockDB);
        }
        //MockDb can't user .Find(id); so the test case is void
        [TestMethod]
        public void TestGetMilestonesByAssignmentID()
        {
            //Arrange:
            const int id1 = 1;
            const int id2 = 2;
            const int id3 = 3;
            const int assignmentID1 = 1;
            const int assignmentID2 = 2;
            const int assignmentID3 = 3;
            const string title1 = "test1";
            const string title2 = "test2";
            const string title3 = "test3";

            //Act:
            var result1 = _service.TestGetMilestonesByAssignmentID(id1);
            var result2 = _service.TestGetMilestonesByAssignmentID(id2);
            var result3 = _service.TestGetMilestonesByAssignmentID(id3);
            //Assert:
            Assert.AreEqual(result1[0].assignmentID, assignmentID1);
            Assert.AreEqual(result2[0].assignmentID, assignmentID2);
            Assert.AreEqual(result3[0].assignmentID, assignmentID3);

            Assert.AreNotEqual(result1[0].title, title2);
            Assert.AreNotEqual(result2[0].title, title3);
            Assert.AreNotEqual(result3[0].title, title1);
        }
    }
}
