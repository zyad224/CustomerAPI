using CustomerAPI.Dal;
using CustomerAPI.Dal.Interfaces;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.UnitTests
{
   public class UserDalTest
    {
        private IUserDal _userDal;
        private DbApiContext _dbApiContext;
        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<DbApiContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            _dbApiContext = new DbApiContext(options);
        }

        [Test]
        public async Task AddUser_Returns_NewCustomerId()
        {

            //Arrange
            _userDal = new UserDal(_dbApiContext);
             UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "Smith" };

            //Act
            var userDb = await _userDal.AddUser(userInfoModel);

            //Assert
            Assert.IsTrue(userInfoModel.FirstName == userDb.FirstName);
            Assert.IsTrue(userInfoModel.SureName == userDb.SureName);
            Assert.IsTrue(!string.IsNullOrEmpty(userDb.CustomerId));

        }

        [Test]
        public async Task Add2DuplicateNamesUsers_Returns_2UniqueCustomerIds()
        {

            //Arrange
            _userDal = new UserDal(_dbApiContext);
            UserInfoModel userInfoModel1 = new UserInfoModel() { FirstName = "John", SureName = "Smith" };
            UserInfoModel userInfoModel2 = new UserInfoModel() { FirstName = "John", SureName = "Smith" };


            //Act
            var userDb1 = await _userDal.AddUser(userInfoModel1);
            var userDb2 = await _userDal.AddUser(userInfoModel2);

            //Assert
            Assert.IsTrue(!string.IsNullOrEmpty(userDb1.CustomerId));
            Assert.IsTrue(!string.IsNullOrEmpty(userDb2.CustomerId));
            Assert.IsTrue(userDb1.CustomerId != userDb2.CustomerId);



        }

        [Test]
        public async Task AddUser_GetUserInfo_Returns_EmptyAccounts()
        {

            //Arrange
            _userDal = new UserDal(_dbApiContext);
            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "Smith" };


            //Act
            var userDb = await _userDal.AddUser(userInfoModel);
            UserInfoModel userInfoModelDb = new UserInfoModel() { CustomerId = userDb.CustomerId, FirstName = userDb.FirstName, SureName = userDb.SureName };
            var userInfoDb = await _userDal.UserInfo(userInfoModelDb);

            //Assert
            Assert.IsTrue(userInfoModelDb.FirstName == userInfoDb.FirstName);
            Assert.IsTrue(userInfoModelDb.SureName == userInfoDb.SureName);
            Assert.IsTrue(userInfoModelDb.CustomerId == userInfoDb.CustomerId);
            Assert.IsTrue(userInfoDb.Accounts.Count == 0);





        }
    }
}
