using CustomerAPI.Controllers;
using CustomerAPI.Core.Interfaces;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerAPI.IntegrationTests.TransactionControllerTest
{

    public class TransactionControllerTest
    {
        private Mock<ITransactionService> _mockTransactionService;

        [SetUp]
        public void SetUp()
        {
            _mockTransactionService = new Mock<ITransactionService>();

        }


        [Test]
        public async Task Deposit_SubmitNullCusomterId_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "", AccountId = "123", Amount = 100 };
            var result = await transactionController.Deposit(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null CustomerId or Null Account");
        }
        [Test]
        public async Task Deposit_SubmitNullAccountId_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "", Amount = 100 };
            var result = await transactionController.Deposit(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null CustomerId or Null Account");
        }
        [Test]
        public async Task Deposit_AmountEqualsToZero_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "123", Amount = 0 };
            var result = await transactionController.Deposit(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Enter Amount > 0");
        }
        [Test]
        public async Task Deposit_NegativeAmount_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "123", Amount = -100 };
            var result = await transactionController.Deposit(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Enter Amount > 0");
        }

        [Test]
        public async Task ValidDeposit_Returns_OK()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "123", Amount = 100 };

            _mockTransactionService.Setup(transactionService => transactionService.DepositTransaction(transactionModel))
                .Returns(Task.FromResult(new TransactionReturnModel() { TransactionId = "NewTransaction", TransactionType=Entities.TransactionType.Deposit}));
            var result = await transactionController.Deposit(transactionModel);
            var actualStatusCode = (result.Result as OkObjectResult).StatusCode;
            var actualReturnValue = (result.Result as OkObjectResult).Value;
            TransactionReturnModel actualReturnValueDeseralized = JsonSerializer.Deserialize<TransactionReturnModel>(actualReturnValue.ToString());

            //Assert
            Assert.IsTrue(actualStatusCode == 200);
            Assert.IsTrue(actualReturnValueDeseralized != null);
        }
        [Test]
        public async Task Withdraw_SubmitNullCusomterId_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "", AccountId = "123", Amount = -100 };
            var result = await transactionController.Withdraw(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null CustomerId or Null Account");
        }
        [Test]
        public async Task Withdraw_SubmitNullAccountId_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "", Amount = -100 };
            var result = await transactionController.Withdraw(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null CustomerId or Null Account");
        }
        [Test]
        public async Task Withdraw_AmountEqualsToZero_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "123", Amount = 0 };
            var result = await transactionController.Withdraw(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Enter Amount < 0");
        }
        [Test]
        public async Task Withdraw_PositiveAmount_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "123", Amount = 100 };
            var result = await transactionController.Withdraw(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Enter Amount < 0");
        }
        [Test]
        public async Task ValidWithdraw_Returns_OK()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "123", Amount = -100 };

            _mockTransactionService.Setup(transactionService => transactionService.WithDrawTransaction(transactionModel))
                .Returns(Task.FromResult(new TransactionReturnModel() { TransactionId = "NewTransaction", TransactionType = Entities.TransactionType.Withdraw }));
            var result = await transactionController.Withdraw(transactionModel);
            var actualStatusCode = (result.Result as OkObjectResult).StatusCode;
            var actualReturnValue = (result.Result as OkObjectResult).Value;
            TransactionReturnModel actualReturnValueDeseralized = JsonSerializer.Deserialize<TransactionReturnModel>(actualReturnValue.ToString());

            //Assert
            Assert.IsTrue(actualStatusCode == 200);
            Assert.IsTrue(actualReturnValueDeseralized != null);
        }
        [Test]
        public async Task CheckBalance_SubmitNullCusomterId_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "", AccountId = "123"};
            var result = await transactionController.CheckBalance(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null CustomerId or Null Account");
        }
        [Test]
        public async Task CheckBalance_SubmitNullAccountId_Returns_BadRequest()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "" };
            var result = await transactionController.CheckBalance(transactionModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null CustomerId or Null Account");
        }
        [Test]
        public async Task VaildCheckBalance_Returns_OK()
        {
            //Arrange
            var transactionController = new TransactionController(_mockTransactionService.Object);

            //Act
            TransactionModel transactionModel = new TransactionModel() { CustomerId = "abc", AccountId = "123" };
            _mockTransactionService.Setup(transactionService => transactionService.CheckBalanceTransaction(transactionModel))
               .Returns(Task.FromResult(new TransactionReturnModel() { TransactionId = "NewTransaction", TransactionType = Entities.TransactionType.CheckBalance }));
            var result = await transactionController.CheckBalance(transactionModel);
            var actualStatusCode = (result.Result as OkObjectResult).StatusCode;
            var actualReturnValue = (result.Result as OkObjectResult).Value;
            TransactionReturnModel actualReturnValueDeseralized = JsonSerializer.Deserialize<TransactionReturnModel>(actualReturnValue.ToString());

            //Assert
            Assert.IsTrue(actualStatusCode == 200);
            Assert.IsTrue(actualReturnValueDeseralized!=null);
        }

    }
}
