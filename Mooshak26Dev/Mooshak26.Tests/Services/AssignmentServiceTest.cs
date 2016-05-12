using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mooshak26.Models.Entities;
using Mooshak26.Services;


namespace Mooshak26.Tests.Services
{
    [TestClass]
    public class AssignmentServiceTest
    {
        /// <summary>
        /// The test cases are located at the bottom of CourseService.cs 
        /// They talk to _mockDB instead of _db...
        /// </summary>
        private AssignmentService _service;
        [TestInitialize]
        public void Initialize()
        {
            var mockDB = new MockDataContext();
            _service = new AssignmentService();
            var a1 = new Assignment
            {
                id = 1,
                courseID = 1,
                title = "test1",
                description = "test1",
                totalGrade = 10

            };
            mockDB.Assignments1.Add(a1);
            var a2 = new Assignment
            {
                id = 2,
                courseID = 2,
                title = "test2",
                description = "test2",
                totalGrade = 9
            };
            mockDB.Assignments1.Add(a2);

            var a3 = new Assignment
            {
                id = 3,
                courseID = 3,
                title = "test3",
                description = "test3",
                totalGrade = 8
            };
            mockDB.Assignments1.Add(a3);
            _service = new AssignmentService(mockDB);
        }
        //MockDb can't user .Find(id); so the test case is void
        //[TestMethod]
        public void TestGetAssignmentDetails()
        {
            //Arrange:
            const int id1 = 1;
            const int id2 = 2;
            const int id3 = 3;
            const string title1 = "test1";
            const string title2 = "test2";
            const string title3 = "test3";

            //Act:
            var result1 = _service.TestGetAssignmentDetails(id1);
            var result2 = _service.TestGetAssignmentDetails(id2);
            var result3 = _service.TestGetAssignmentDetails(id3);
            //Assert:
            Assert.AreEqual(result1.title, title1);
            Assert.AreEqual(result2.title, title2);
            Assert.AreEqual(result3.title, title2);
            Assert.AreNotEqual(result1.title, title2);
            Assert.AreNotEqual(result2.title, title3);
            Assert.AreNotEqual(result3.title, title1);
        }
    }
}
