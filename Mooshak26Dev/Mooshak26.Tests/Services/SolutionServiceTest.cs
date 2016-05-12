using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mooshak26.Models.Entities;
using Mooshak26.Services;


namespace Mooshak26.Tests.Services
{
    [TestClass]
    public class SolutionServiceTest
    {
        private SolutionService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDB = new MockDataContext();
            _service = new SolutionService();
            var a1 = new Solution
            {
                Id = 1,
                courseID = 1, 
                assignmentID = 1, 
                milestoneID = 1,
                input = "test1", 
                output = "test1"

            };
            mockDB.Solutions.Add(a1);
            var a2 = new Solution
            {
                Id = 2,
                courseID = 2,
                assignmentID = 2,
                milestoneID = 2,
                input = "test2",
                output = "test2"
            };
            mockDB.Solutions.Add(a2);

            var a3 = new Solution
            {
                Id = 3,
                courseID = 3,
                assignmentID = 3,
                milestoneID = 3,
                input = "test3",
                output = "test3"
            };
            mockDB.Solutions.Add(a3);
            _service = new SolutionService(mockDB);
        }
        //MockDb can't user .Find(id); so the test case is void
        [TestMethod]
        public void TestGetAllSolutions()
        {
            //Arrange:
            const int id1 = 1;
            const int id2 = 2;
            const int id3 = 3;
            const int courseID1 = 1;
            const int assignmentID2 = 2;
            const int milestoneID3 = 3;
            const string input1 = "test1";
            const string input2 = "test2";
            const string input3 = "test3";

            //Act:
            var result1 = _service.TestGetAllSolutions(id1);
            var result2 = _service.TestGetAllSolutions(id2);
            var result3 = _service.TestGetAllSolutions(id3);
            //Assert:
            Assert.AreEqual(result1[0].courseID, courseID1);
            Assert.AreEqual(result2[0].assignmentID, assignmentID2);
            Assert.AreEqual(result3[0].milestoneID, milestoneID3);

            Assert.AreNotEqual(result1[0].input, input2);
            Assert.AreNotEqual(result2[0].input, input3);
            Assert.AreNotEqual(result3[0].input, input1);

            Assert.AreNotEqual(result1[0].output, input2);
            Assert.AreNotEqual(result2[0].output, input3);
            Assert.AreNotEqual(result3[0].output, input1);
        }
    }
}

