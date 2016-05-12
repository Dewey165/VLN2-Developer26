using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mooshak26.Models.Entities;
using Mooshak26.Services;


namespace Mooshak26.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private UserService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDB = new MockDataContext();
            _service = new UserService();
            var a1 = new User
            {
                id = 1,
                userName = "test1",
                email = "test1@test1.is",
                role = "Test1"

            };
            mockDB.MyUsers.Add(a1);
            var a2 = new User
            {
                id = 2,
                userName = "test2",
                email = "test2@test2.is",
                role = "Test2"
            };
            mockDB.MyUsers.Add(a2);

            var a3 = new User
            {
                id = 3,
                userName = "test3",
                email = "test3@test3.is",
                role = "Test3"
            };
            mockDB.MyUsers.Add(a3);
            _service = new UserService(mockDB);
        }
        //MockDb can't user .Find(id); so the test case is void
        [TestMethod]
        public void TestGetUsers()
        {
            //Arrange:
            const int id1 = 1;
            const int id2 = 2;
            const int id3 = 3;
            const string user1 = "test1";
            const string userRole2 = "Test2";
            const string user3 = "test3";
            const string user3Email = "test3@test3.is";


            //Act:
            var result = _service.TestGetUsers();
           
            //Assert:
            Assert.AreEqual(result[0].userName, user1);
            Assert.AreEqual(result[1].role, userRole2);
            Assert.AreEqual(result[2].email, user3Email);

            Assert.AreNotEqual(result[0].userName, user3);
            Assert.AreNotEqual(result[1].role, user3);
            Assert.AreNotEqual(result[2].email, user1);

            Assert.AreNotEqual(result[0].id, id2);
            Assert.AreNotEqual(result[1].id, id3);
            Assert.AreNotEqual(result[2].id, id1);
        }
        [TestMethod]
        public void TestGetRole()
        {
            //Arrange:
            const int id1 = 1;
            const int id2 = 2;
            const int id3 = 3;
            const string userRole1 = "Test1";
            const string userRole2 = "Test2";
            const string userRole3 = "Test3";
            
            //Act:
            var result1 = _service.TestGetRole(id1);
            var result2 = _service.TestGetRole(id2);
            var result3 = _service.TestGetRole(id3);

            //Assert:
            Assert.AreEqual(result1, userRole1);
            Assert.AreEqual(result2, userRole2);
            Assert.AreEqual(result3, userRole3);

            Assert.AreNotEqual(result1, userRole2);
            Assert.AreNotEqual(result2, userRole3);
            Assert.AreNotEqual(result3, userRole1);

        }
        [TestMethod]
        public void TestFindUserIDByUsername()
        {
            //Arrange:
            const int id1 = 1;
            const int id2 = 2;
            const int id3 = 3;
            const string userName1 = "test1";
            const string userName2 = "test2";
            const string userName3 = "test3";

            //Act:
            var result1 = _service.TestFindUserIDByUsername(userName1);
            var result2 = _service.TestFindUserIDByUsername(userName2);
            var result3 = _service.TestFindUserIDByUsername(userName3);

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


