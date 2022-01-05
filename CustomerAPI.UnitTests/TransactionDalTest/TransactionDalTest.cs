using CustomerAPI.Dal;
using CustomerAPI.Dal.Interfaces;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.UnitTests.TransactionDalTest
{
    public class TransactionDalTest
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
        public async Task CreateDepositTransactionEqualsTen_ZeroInitialCredit_Returns_SavedDepositTransaction_NewBalanceEqualsTen()
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

            TransactionModel transactionModel = new TransactionModel()
            { CustomerId = userInfoDb.CustomerId, AccountId = userInfoDb.Accounts.FirstOrDefault().AccountId, Amount = 10 };

            var transactionDb = await _transactionDal.DepositTransaction(transactionModel);
            userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsNotNull(transactionDb);
            Assert.IsTrue(!string.IsNullOrEmpty(transactionDb.TransactionId));
            Assert.IsTrue(transactionDb.TransactionType == Entities.TransactionType.Deposit);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.Count == 1);
            Assert.IsTrue(!string.IsNullOrEmpty(userInfoDb.Accounts.FirstOrDefault().Transactions.FirstOrDefault().TransactionId));
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Balance == 10);


        }

        [Test]
        public async Task CreateDepositTransactionEqualsTen_TenInitialCredit_Returns_SavedDepositTransaction_NewBalanceEqualsTwenty()
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

            TransactionModel transactionModel = new TransactionModel()
            { CustomerId = userInfoDb.CustomerId, AccountId = userInfoDb.Accounts.FirstOrDefault().AccountId, Amount = 10 };

            var transactionDb = await _transactionDal.DepositTransaction(transactionModel);
            userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsNotNull(transactionDb);
            Assert.IsTrue(!string.IsNullOrEmpty(transactionDb.TransactionId));
            Assert.IsTrue(transactionDb.TransactionType == Entities.TransactionType.Deposit);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.Count == 2);    // open account and deposit
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Balance == 20);


        }

        [Test]
        public async Task CreateWithdrawTransaction_ZeroInitialCredit_Returns_NoTransaction_NewBalanceEqualsZero()
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

            TransactionModel transactionModel = new TransactionModel()
            { CustomerId = userInfoDb.CustomerId, AccountId = userInfoDb.Accounts.FirstOrDefault().AccountId, Amount = -10 };

            var transactionDb = await _transactionDal.WithDrawTransaction(transactionModel);
            userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsNotNull(transactionDb);
            Assert.IsTrue(string.IsNullOrEmpty(transactionDb.TransactionId));
            Assert.IsTrue(transactionDb.TransactionType == Entities.TransactionType.NoTransaction);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.Count == 0);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Balance == 0);


        }

        [Test]
        public async Task CreateWithdrawTransaction_NegativeTenInitialCredit_Returns_NoTransaction_NewBalanceEqualsNegativeTen()
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

            TransactionModel transactionModel = new TransactionModel()
            { CustomerId = userInfoDb.CustomerId, AccountId = userInfoDb.Accounts.FirstOrDefault().AccountId, Amount = -10 };

            var transactionDb = await _transactionDal.WithDrawTransaction(transactionModel);
            userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsNotNull(transactionDb);
            Assert.IsTrue(string.IsNullOrEmpty(transactionDb.TransactionId));
            Assert.IsTrue(transactionDb.TransactionType == Entities.TransactionType.NoTransaction);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.Count == 1);   // because we opened the account with -10 initially
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Balance == -10);


        }

        [Test]
        public async Task CreateCheckBalanceTransaction_TenInitialCredit_Returns_CheckBalanceTransaction_NewBalanceEqualsTen()
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

            TransactionModel transactionModel = new TransactionModel()
            { CustomerId = userInfoDb.CustomerId, AccountId = userInfoDb.Accounts.FirstOrDefault().AccountId, Amount = -10 };

            var transactionDb = await _transactionDal.CheckBalanceTransaction(transactionModel);
            userInfoDb = await _userDal.UserInfo(userInfoModel);

            //Assert
            Assert.IsNotNull(transactionDb);
            Assert.IsTrue(!string.IsNullOrEmpty(transactionDb.TransactionId));
            Assert.IsTrue(transactionDb.TransactionType == Entities.TransactionType.CheckBalance);
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Transactions.Count == 2);    // open account and checkbalance
            Assert.IsTrue(userInfoDb.Accounts.FirstOrDefault().Balance == 10);


        }

    }
}
