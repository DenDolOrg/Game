using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyGame.Models;
using MyGame.Tests.Services;
using MyGame.BLL.DTO;
using System.Web.Helpers;
using MyGame.Tests.MockManagers;
using MyGame.Tests.Models;
using System.Collections.Generic;
using System.Web;

namespace MyGame.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {

        MockAuthenticationManager mockAuthenticationManager;

        [TestInitialize]
        public void Init()
        {
            mockAuthenticationManager = new MockAuthenticationManager()
            .MockSignOut()
            .MockSignIn();

            ControllerDataToUse.SetData();
        }
         

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
            LoginModel loginModel_good = new LoginModel { Email = ControllerDataToUse.UserDTO.Email, Password = ControllerDataToUse.UserDTO.Password };
            LoginModel loginModel_bad_1 = new LoginModel { Email = "email_2@gmail.com", Password = "123456" };

            var mockUserService = new MockUserService()
                .MockAuthenticate();

            //Act
            AccountController accountController = new AccountController(mockUserService.Object, null, mockAuthenticationManager.Object);
            ActionResult goodResult = await accountController.Login(loginModel_good);
            ActionResult badResult_1 = await accountController.Login(loginModel_bad_1);

            //Assert
            Assert.IsInstanceOfType(goodResult, typeof(RedirectToRouteResult), "Good login info Fail.");
            Assert.IsInstanceOfType(badResult_1, typeof(ViewResult), "No Fail while using bad login info.");
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
                Email = "new_email@gmail.com",
                Password = "123456",
                ConfirmPassword = "123456",
                Nickname = "username_1",
                Name = "name",
                Surname = "surname"
            };

            RegisterModel registerModel_bad = new RegisterModel
            {
                Email = ControllerDataToUse.UserDTO.Email,
                Password = ControllerDataToUse.UserDTO.Password,
                ConfirmPassword = ControllerDataToUse.UserDTO.Password,
                Nickname = ControllerDataToUse.UserDTO.UserName,
                Name = ControllerDataToUse.UserDTO.Name,
                Surname = ControllerDataToUse.UserDTO.Surname
            };

            var mockUserService = new MockUserService()
                .MockCreate()
                .MockAuthenticate();

                
            //Act
            AccountController accountController = new AccountController(mockUserService.Object, null, mockAuthenticationManager.Object);
            ActionResult goodResult = await accountController.Register(registerModel_good);
            ActionResult badResult_3 = await accountController.Register(registerModel_bad);

            //Assert
            Assert.IsInstanceOfType(goodResult, typeof(RedirectToRouteResult), "Good Reg info Fail");
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
            UserDTO user_good = new UserDTO { Id = ControllerDataToUse.UserDTO.Id };
            UserDTO user_bad = new UserDTO { Id = 20 };

            var mockUserService = new MockUserService()
                .MockDelete();

            var mockTableService = new MockGameService()
                .MockDeleteUserGames();

            //Act
            AccountController accountController = new AccountController(mockUserService.Object, mockTableService.Object);
            await accountController.Delete(user_good.Id);

            //Assert
            await Assert.ThrowsExceptionAsync<HttpException>( async() => await accountController.Delete(user_bad.Id), "Can delete user with invalid id.");
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
            var mockUserService = new MockUserService()
                .MockGetAllUsers();

            //Act
            AccountController accountController = new AccountController(mockUserService.Object);
            var result = await accountController.GetAllUsers();

            //Assert
            Assert.AreEqual(Json.Encode(new List<UserDTO> { ControllerDataToUse.UserDTO  }), Json.Encode(result.Data), "Not the same Json result.");
        }
        #endregion
    }
}