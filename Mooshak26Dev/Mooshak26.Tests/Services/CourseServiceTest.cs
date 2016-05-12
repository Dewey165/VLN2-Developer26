using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mooshak26.Services;
using Mooshak26.Models.Entities;

namespace Mooshak26.Tests.Services
{
    [TestClass]
    public class CourseServiceTest
    {
        /// <summary>
        /// The test cases are located at the bottom of CourseService.cs 
        /// They talk to _mockDB instead of _db...
        /// </summary>
        private CourseService _service;
        [TestInitialize]
        public void Initialize()
        {
            var mockDB = new MockDataContext();
            _service = new CourseService();
            var c1 = new Course
            {
                id = 1,
                title = "test1",
                description = "test1"
            };
            mockDB.courses.Add(c1);
            var c2 = new Course
            {
                id = 2,
                title = "test2",
                description = "test2"
            };
            mockDB.courses.Add(c2);

            var c3 = new Course
            {
                id = 3,
                title = "test3",
                description = "test3"
            };
            mockDB.courses.Add(c3);
            _service = new CourseService(mockDB);
        }
        [TestMethod]
        public void TestGetCourses()
        {
            //Arrange:
            const string title = "test1";
            
            //Act:
            var result = _service.TestGetCourses();
            //Assert:
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(result[0].title, title);
            Assert.AreEqual(result[0].description, title);
            Assert.AreNotEqual(result[1].title, title);
            Assert.AreNotEqual(result[2].title, title);
        }
        [TestMethod]
        public void TestFindCourseIDByName()
        {
            //Arrange:
            const string title1 = "test1";
            const string title2 = "test2";
            const string title3 = "test3";
            const int id1 = 1;
            const int id2 = 2;
            const int id3 = 3;
            //Act:
            int result1 = _service.TestFindCourseIDByName(title1);
            int result2 = _service.TestFindCourseIDByName(title2);
            int result3 = _service.TestFindCourseIDByName(title3);
            //Assert:
            Assert.AreEqual(result1, id1);
            Assert.AreEqual(result2, id2);
            Assert.AreEqual(result3, id3);

            Assert.AreNotEqual(result1, id2);
            Assert.AreNotEqual(result2, id3);
            Assert.AreNotEqual(result3, id1);


        }
    }
}
