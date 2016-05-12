using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mooshak26.Models.Entities;
using Mooshak26.Services;


namespace Mooshak26.Tests.Services
{
    [TestClass]
    public class LinkServiceTest
    {
        private LinkService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDB = new MockDataContext();
            _service = new LinkService();
            var a1 = new Link
            {
                id = 1,
                userID = 1,
                userName = "test1",
                courseID = 1,
                courseName = "test1",
                role = "Student"
            };
            mockDB.Links.Add(a1);
            var a2 = new Link
            {
                id = 2,
                userID = 2,
                userName = "test2",
                courseID = 2,
                courseName = "test2",
                role = "Student"
            };
            mockDB.Links.Add(a2);

            var a3 = new Link
            {
                id = 3,
                userID = 3,
                userName = "test3",
                courseID = 3,
                courseName = "test3",
                role = "Student"
            };
            mockDB.Links.Add(a3);
            _service = new LinkService(mockDB);
        }
        //MockDb can't user .Find(id); so the test case is void
        [TestMethod]
        public void TestUserLinks()
        {
            //Arrange:
            const int id1 = 1;
            const int id2 = 2;
            const int id3 = 3;
            const int courseID1 = 1;
            const int courseID2 = 2;
            const int courseID3 = 3;


            //Act:
            var result1 = _service.TestUserLinks(id1);
            var result2 = _service.TestUserLinks(id2);
            var result3 = _service.TestUserLinks(id3);
            //Assert:
            Assert.AreEqual(result1[0], courseID1);
            Assert.AreEqual(result2[0], courseID2);
            Assert.AreEqual(result3[0], courseID3);
            
            Assert.AreNotEqual(result1[0], courseID2);
            Assert.AreNotEqual(result2[0], courseID3);
            Assert.AreNotEqual(result3[0], courseID1);
        }
        [TestMethod]
        public void TestGetLinks()
        {
            //Arrange:
            const string title = "test1";
            const int courseID1 = 1;


            //Act:
            var result = _service.TestGetLinks();
            //Assert:
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(result[0].courseID, courseID1);
            Assert.AreEqual(result[0].courseName, title);
            Assert.AreNotEqual(result[1].courseName, title);
            Assert.AreNotEqual(result[2].courseName, title);

        }
    }
}
