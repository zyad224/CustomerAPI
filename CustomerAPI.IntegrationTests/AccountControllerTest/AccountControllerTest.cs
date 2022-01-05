using CustomerAPI.Controllers;
using CustomerAPI.Core.Interfaces;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerAPI.IntegrationTests.AccountControllerTest
{
    public class AccountControllerTest
    {
        private Mock<IAccountService> _mockAccountService;

        [SetUp]
        public void SetUp()
        {
            _mockAccountService = new Mock<IAccountService>();

        }

        [Test]
        public async Task OpenAccount_SubmitNullCusomterId_Returns_BadRequest()
        {
            //Arrange
            var accountController = new AccountController(_mockAccountService.Object);

            //Act
            UserInfoModel userInfoModel = new UserInfoModel() { CustomerId = ""};
            var result = (BadRequestObjectResult) await accountController.OpenNewAccountExistingUser(userInfoModel);          
            var actualStatusCode = result.StatusCode;
            var actualReturnValue = result.Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Bad CustomerId");
        }
        [Test]
        public async Task OpenAccount_ForNonExistingUser_Returns_BadRequest()
        {
            //Arrange
            var accountController = new AccountController(_mockAccountService.Object);

            //Act
            UserInfoModel userInfoModel = new UserInfoModel() { CustomerId = "NonExisting" };
            _mockAccountService.Setup(accountService => accountService.OpenNewAccountExistingUser(userInfoModel))
               .Returns(Task.FromResult(false));

            var result = (BadRequestObjectResult) await accountController.OpenNewAccountExistingUser(userInfoModel);
            var actualStatusCode = result.StatusCode;
            var actualReturnValue = result.Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue((bool)actualReturnValue == false);
        }
        [Test]
        public async Task OpenAccount_ForExistingUser_Returns_OK()
        {
            //Arrange
            var accountController = new AccountController(_mockAccountService.Object);

            //Act
            UserInfoModel userInfoModel = new UserInfoModel() { CustomerId = "Existing" };
            _mockAccountService.Setup(accountService => accountService.OpenNewAccountExistingUser(userInfoModel))
               .Returns(Task.FromResult(true));

            var result = (OkObjectResult)await accountController.OpenNewAccountExistingUser(userInfoModel);
            var actualStatusCode = result.StatusCode;
            var actualReturnValue = result.Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 200);
            Assert.IsTrue((bool)actualReturnValue == true);
        }
    }
}
