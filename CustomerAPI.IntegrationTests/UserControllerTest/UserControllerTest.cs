using CustomerAPI.Controllers;
using CustomerAPI.Core.Interfaces;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerAPI.IntegrationTests
{
    public class UserControllerTest
    {
        private Mock<IUserService> _mockUserService;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            
        }

        [Test]
        public async Task AddUser_NullFirstName_Returns_BadRequest()
        {
            //Arrange
            var userController = new UserController(_mockUserService.Object);

            //Act
            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "", SureName = "Smith" };
            var result = await userController.AddUser(userInfoModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null FirstName or Null SureName");
        }

        [Test]
        public async Task AddUser_NullSureName_Returns_BadRequest()
        {
            //Arrange
            var userController = new UserController(_mockUserService.Object);

            //Act
            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "" };
            var result = await userController.AddUser(userInfoModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null FirstName or Null SureName");
        }

        [Test]
        public async Task AddUser_ValidFirstName_ValidSureName_Returns_OK()
        {
            //Arrange
            var userController = new UserController(_mockUserService.Object);

            //Act
            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "Smith" };
            _mockUserService.Setup(userService => userService.AddUser(userInfoModel))
                .Returns(Task.FromResult(new UserInfoReturnModel() {FirstName = "John", SureName = "Smith" }));
            var result = await userController.AddUser(userInfoModel);
            var actualStatusCode = (result.Result as OkObjectResult).StatusCode;
            var actualReturnValue = (result.Result as OkObjectResult).Value;
            UserInfoReturnModel actualReturnValueDeseralized = JsonSerializer.Deserialize<UserInfoReturnModel>(actualReturnValue.ToString());

            //Assert
            Assert.IsTrue(actualStatusCode == 200);
            Assert.IsTrue(actualReturnValueDeseralized.FirstName == "John");
            Assert.IsTrue(actualReturnValueDeseralized.SureName == "Smith");

        }

        [Test]
        public async Task UserInfo_NullCustomerId_Returns_BadRequest()
        {
            //Arrange
            var userController = new UserController(_mockUserService.Object);

            //Act
            UserInfoModel userInfoModel = new UserInfoModel() { CustomerId = "" };         
            var result = await userController.UserInfo(userInfoModel);
            var actualStatusCode = (result.Result as BadRequestObjectResult).StatusCode;
            var actualReturnValue = (result.Result as BadRequestObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 400);
            Assert.IsTrue(actualReturnValue.ToString() == "Null CustomerId");

        }
        [Test]
        public async Task UserInfo_NonExistingUser_Returns_NotFound()
        {
            //Arrange
            var userController = new UserController(_mockUserService.Object);

            //Act
            UserInfoReturnModel userInfoReturnModel = null;
            UserInfoModel userInfoModel = new UserInfoModel() { CustomerId = "NonExisting" };
            _mockUserService.Setup(userService => userService.UserInfo(userInfoModel))
                .Returns(Task.FromResult(userInfoReturnModel));
            var result = await userController.UserInfo(userInfoModel);
            var actualStatusCode = (result.Result as NotFoundObjectResult).StatusCode;
            var actualReturnValue = (result.Result as NotFoundObjectResult).Value;

            //Assert
            Assert.IsTrue(actualStatusCode == 404);
            Assert.IsTrue(actualReturnValue.ToString() == "No Such Customer Exists");

        }

        [Test]
        public async Task UserInfo_ExistingUser_Returns_OK()
        {
            //Arrange
            var userController = new UserController(_mockUserService.Object);

            //Act
            UserInfoModel userInfoModel = new UserInfoModel() { FirstName = "John", SureName = "Smith" };
            UserInfoReturnModel userInfoReturnModel = new UserInfoReturnModel { CustomerId = "123", FirstName = "John", SureName = "Smith" };

            _mockUserService.Setup(userService => userService.AddUser(userInfoModel))
                .Returns(Task.FromResult(new UserInfoReturnModel { CustomerId = "123", FirstName = "John", SureName = "Smith" }));

            userInfoModel.CustomerId = userInfoReturnModel.CustomerId;

            _mockUserService.Setup(userService => userService.UserInfo(userInfoModel))
                .Returns(Task.FromResult(new UserInfoReturnModel()));

            var result = await userController.UserInfo(userInfoModel);

            var actualStatusCode = (result.Result as OkObjectResult).StatusCode;
            var actualReturnValue = (result.Result as OkObjectResult).Value;
            UserInfoReturnModel actualReturnValueDeseralized = JsonSerializer.Deserialize<UserInfoReturnModel>(actualReturnValue.ToString());

            //Assert
            Assert.IsTrue(actualStatusCode == 200);
            Assert.IsTrue(actualReturnValueDeseralized!=null);

        }
    }
}