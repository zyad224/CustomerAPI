using CustomerAPI.Dal;
using CustomerAPI.Dal.Interfaces;
using CustomerAPI.Entities;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.UnitTests.AccountDalTest
{
    public class AccountDalTest
    {

        private IAccountDal _accountDal;
        private IUserDal _userDal;
        private ITransactionDal _transactionDal;
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
        public async Task OpenNewAccount_NonExistingUser_Returns_False()
        {

            //Arrange
            _userDal = new UserDal(_dbApiContext);
            _transactionDal = new TransactionDal(_dbApiContext);
            _accountDal = new AccountDal(_dbApiContext, _transactionDal);
            
            UserInfoModel userInfoModel = new UserInfoModel() { CustomerId = "NonExisting"};

            //Act
            var accountAdded = await _accountDal.OpenNewAccountExistingUser(userInfoModel);

            //Assert
            Assert.IsFalse(accountAdded);
         

        }

        [Test]
        public async Task OpenNewAccount_ExistingUser_PostiveInitialCredit_Returns_NewAccount_WithDepositTransaction()
        {

            //Arrange
            _userDal = new UserDal(_dbApiContext);
            _transactionDal = new TransactionDal(_dbApiContext);
            _accountDal = new AccountDal(_dbApiContext, _transactionDal);

            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "Smith" };
            var userDb = await _userDal.AddUser(userInfoModel);
            userInfoModel.InitialCredit = 10;
            userInfoModel.CustomerId = userDb.CustomerId;

            //Act
            var accountAdded = await _accountDal.OpenNewAccountExistingUser(userInfoModel);
            var userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsTrue(accountAdded);
            Assert.IsTrue(userInfoDb.CustomerId == userInfoModel.CustomerId);
            Assert.IsTrue(userInfoDb.Accounts.Count == 1);
            Assert.IsTrue(!string.IsNullOrEmpty(userInfoDb.Accounts.FirstOrDefault().AccountId));
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Balance == 10);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.Count==1);
            Assert.IsTrue(!string.IsNullOrEmpty(userInfoDb.Accounts.FirstOrDefault().Transactions.FirstOrDefault().TransactionId));
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.FirstOrDefault().TransactionType == TransactionType.Deposit);

        }

        [Test]
        public async Task OpenNewAccount_ExistingUser_NegativeInitialCredit_Returns_NewAccount_WithWithDrawTransaction()
        {

            //Arrange
            _userDal = new UserDal(_dbApiContext);
            _transactionDal = new TransactionDal(_dbApiContext);
            _accountDal = new AccountDal(_dbApiContext, _transactionDal);

            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "Smith" };
            var userDb = await _userDal.AddUser(userInfoModel);
            userInfoModel.InitialCredit = -10;
            userInfoModel.CustomerId = userDb.CustomerId;

            //Act
            var accountAdded = await _accountDal.OpenNewAccountExistingUser(userInfoModel);
            var userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsTrue(accountAdded);
            Assert.IsTrue(userInfoDb.CustomerId == userInfoModel.CustomerId);
            Assert.IsTrue(userInfoDb.Accounts.Count == 1);
            Assert.IsTrue(!string.IsNullOrEmpty(userInfoDb.Accounts.FirstOrDefault().AccountId));
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Balance == -10);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.Count == 1);
            Assert.IsTrue(!string.IsNullOrEmpty(userInfoDb.Accounts.FirstOrDefault().Transactions.FirstOrDefault().TransactionId));
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.FirstOrDefault().TransactionType == TransactionType.Withdraw);

        }


        [Test]
        public async Task OpenNewAccount_ExistingUser_ZeroInitialCredit_Returns_NewAccount_WithNoTransaction()
        {

            //Arrange
            _userDal = new UserDal(_dbApiContext);
            _transactionDal = new TransactionDal(_dbApiContext);
            _accountDal = new AccountDal(_dbApiContext, _transactionDal);

            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "Smith" };
            var userDb = await _userDal.AddUser(userInfoModel);
            userInfoModel.InitialCredit = 0;
            userInfoModel.CustomerId = userDb.CustomerId;

            //Act
            var accountAdded = await _accountDal.OpenNewAccountExistingUser(userInfoModel);
            var userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsTrue(accountAdded);
            Assert.IsTrue(userInfoDb.CustomerId == userInfoModel.CustomerId);
            Assert.IsTrue(userInfoDb.Accounts.Count == 1);
            Assert.IsTrue(!string.IsNullOrEmpty(userInfoDb.Accounts.FirstOrDefault().AccountId));
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Balance == 0);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.Count == 0);

        }

        [Test]
        public async Task OpenTwoNewAccounts_ExistingUser_Returns_TwoUniqueAccounts_SameUser()
        {

            //Arrange
            _userDal = new UserDal(_dbApiContext);
            _transactionDal = new TransactionDal(_dbApiContext);
            _accountDal = new AccountDal(_dbApiContext, _transactionDal);

            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "Smith" };
            var userDb = await _userDal.AddUser(userInfoModel);
            userInfoModel.InitialCredit = 0;
            userInfoModel.CustomerId = userDb.CustomerId;

            //Act
            var accountAdded1 = await _accountDal.OpenNewAccountExistingUser(userInfoModel);
            var accountAdded2 = await _accountDal.OpenNewAccountExistingUser(userInfoModel);
            var userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsTrue(accountAdded1);
            Assert.IsTrue(accountAdded2);
            Assert.IsTrue(userInfoDb.CustomerId == userInfoModel.CustomerId);
            Assert.IsTrue(userInfoDb.Accounts.Count == 2);
           
        }

    }
}
