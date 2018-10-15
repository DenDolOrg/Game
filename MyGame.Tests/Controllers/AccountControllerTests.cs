using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyGame.Models;

using MyGame.Tests.Services;
using MyGame.BLL.DTO;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace MyGame.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        #region DATA_TO_USE

        //List<UserDTO> users = new List<UserDTO>
        //    {
        //        new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
        //        new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
        //        new UserDTO{ Id = 3, Email = "email_3@gmail.com", Password = "333333", UserName = "username_3", Name = "name_3", Surname = "Surname_3"},
        //        new UserDTO{ Id = 4, Email = "email_4@gmail.com", Password = "444444", UserName = "username_4", Name = "name_4", Surname = "Surname_4"}
        //    };

        MockAuthenticationManager mockAuthenticationManager = new MockAuthenticationManager()
            .MockSignOut()
            .MockSignIn();
        #endregion

        #region LOGIN_GET
        [TestMethod()]
        public void LoginGetTest()
        {
            //Arrange
            AccountController accountController = new AccountController(null);
            
            //Act
            ActionResult result = accountController.Login();
            
            //Assert
            Assert.IsNotNull(result, "Showing login page Fail.");
        }
        #endregion

        #region LOGIN_POST
        [TestMethod()]
        public async Task LoginPOSTTest()
        {
            //Arrange
            LoginModel loginModel_good = new LoginModel { Email = "email_1@gmail.com", Password = "111111" };
            LoginModel loginModel_bad_1 = new LoginModel { Email = "email_2@gmail.com", Password = "123456" };
            LoginModel loginModel_bad_2 = new LoginModel { Email = "test_bad@mail.com", Password = "333333" };
            LoginModel loginModel_bad_3 = new LoginModel { Email = "test_bad@mail.com", Password = "123456" };

            var mockUserService = new MockUserService()
                .MockAuthenticate();

            //Act
            AccountController accountController = new AccountController(mockUserService.Object, null, mockAuthenticationManager.Object);
            ActionResult goodResult = await accountController.Login(loginModel_good);
            ActionResult badResult_1 = await accountController.Login(loginModel_bad_1);
            ActionResult badResult_2 = await accountController.Login(loginModel_bad_2);
            ActionResult badResult_3 = await accountController.Login(loginModel_bad_3);

            //Assert
            Assert.IsInstanceOfType(goodResult, typeof(RedirectToRouteResult), "Good login info Fail.");
            Assert.IsInstanceOfType(badResult_1, typeof(ViewResult), "No Fail while using bad login info.");
            Assert.IsInstanceOfType(badResult_2, typeof(ViewResult), "No Fail while using bad login info.");
            Assert.IsInstanceOfType(badResult_3, typeof(ViewResult), "No Fail while using bad login info.");
        }
        #endregion

        #region REGISTER_GET
        [TestMethod()]
        public void RegisterGETTest()
        {
            //Arrange
            AccountController accountController = new AccountController(null);

            //Act
            ActionResult result = accountController.Register();
            
            //Assert
            Assert.IsNotNull(result, "Showing register page Fail.");
        }
        #endregion

        #region REGISTER_POST
        [TestMethod()]
        public async Task RegisterPOSTTest()
        {
            //Arrange
            RegisterModel registerModel_good = new RegisterModel
            {
                Email = "test@mail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Nickname = "nickname",
                Name = "name",
                Surname = "surname"
            };

            RegisterModel registerModel_bad_1 = new RegisterModel
            {
                Email = "email_3@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Nickname = "nickname",
                Name = "name",
                Surname = "surname"
            };

            RegisterModel registerModel_bad_2 = new RegisterModel
            {
                Email = "test@mail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Nickname = "username_2",
                Name = "name",
                Surname = "surname"
            };

            RegisterModel registerModel_bad_3 = new RegisterModel
            {
                Email = "email_1@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Nickname = "username_1",
                Name = "name",
                Surname = "surname"
            };

            var mockUserService = new MockUserService()
                .MockCreate()
                .MockAuthenticate();

            //mockModelState.Setup(s => s.IsValid).Returns(true);
                
            //Act
            AccountController accountController = new AccountController(mockUserService.Object, null, mockAuthenticationManager.Object);
            ActionResult goodResult = await accountController.Register(registerModel_good);
            ActionResult badResult_1 = await accountController.Register(registerModel_bad_1);
            ActionResult badResult_2 = await accountController.Register(registerModel_bad_2);
            ActionResult badResult_3 = await accountController.Register(registerModel_bad_3);

            //Assert
            Assert.IsInstanceOfType(goodResult, typeof(RedirectToRouteResult), "Good Reg info Fail");
            Assert.IsInstanceOfType(badResult_1, typeof(ViewResult), "No Fail with bad info.");
            Assert.IsInstanceOfType(badResult_2, typeof(ViewResult), "No Fail with bad info.");
            Assert.IsInstanceOfType(badResult_3, typeof(ViewResult), "No Fail with bad info.");
        }
        #endregion

        #region LOGOUT
        [TestMethod()]
        public void LogoutTest()
        {
            //Arrange
            AccountController accountController = new AccountController(null, null, mockAuthenticationManager.Object);

            //Act 
            ActionResult result = accountController.Logout();

            //Assert
            Assert.IsNotNull(result);
        }
        #endregion

        #region DELETE
        [TestMethod()]
        public async Task DeleteTest()
        {
            //Arrange
            UserDTO user_good = new UserDTO { Id = 1 };
            UserDTO user_bad = new UserDTO { Id = 20 };

            var mockUserService = new MockUserService()
                .MockDelete();

            var mockTableService = new MockTableService()
                .MockDeleteUserTables();

            //Act
            AccountController accountController = new AccountController(mockUserService.Object, mockTableService.Object);
            bool goodResult = await accountController.Delete(user_good.Id);
            bool badResult = await accountController.Delete(user_bad.Id);

            //Assert
            Assert.IsTrue(goodResult, "Failed when it was good user Id");
            Assert.IsFalse(badResult, "Passed when it was bad user Id");
        }
        #endregion

        #region USER_LIST
        [TestMethod()]
        public void UserListTest()
        {
            //Arrange
            AccountController accountController = new AccountController(null);

            //Act
            ActionResult result = accountController.UserList();

            //Assert
            Assert.IsNotNull(result);
        }
        #endregion

        #region GET_ALL_USERS
        [TestMethod()]
        public async Task GetAllUsersTest()
        {
            //Arrange
            List<UserDTO> users = new List<UserDTO>
            {
                new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
                new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
                new UserDTO{ Id = 3, Email = "email_3@gmail.com", Password = "333333", UserName = "username_3", Name = "name_3", Surname = "Surname_3"},
                new UserDTO{ Id = 4, Email = "email_4@gmail.com", Password = "444444", UserName = "username_4", Name = "name_4", Surname = "Surname_4"}
            };

            var mockUserService = new MockUserService()
                .GetAllUsers();

            //Act
            AccountController accountController = new AccountController(mockUserService.Object);
            var result = await accountController.GetAllUsers();

            //Assert
            Assert.AreEqual(Json.Encode(users), Json.Encode(result.Data), "Not the same Json result.");
        }
        #endregion
    }
}