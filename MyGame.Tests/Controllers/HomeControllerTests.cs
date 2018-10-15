using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyGame.Tests.Services;
using System.Web;
using Moq;
using MyGame.Infrastructure;
using MyGame.BLL.DTO;
using System.Security.Principal;
using System.Web.Routing;
using MyGame.Tests.MockHelpers;

namespace MyGame.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        #region DATA_TO_USE
        //List<UserDTO> users = new List<UserDTO>
        //    {
        //        new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
        //        new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
        //        new UserDTO{ Id = 3, Email = "email_3@gmail.com", Password = "333333", UserName = "username_3", Name = "name_3", Surname = "Surname_3"},
        //        new UserDTO{ Id = 4, Email = "email_4@gmail.com", Password = "444444", UserName = "username_4", Name = "name_4", Surname = "Surname_4"}
        //    };

        #endregion

        #region INDEX
        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            UserDTO user = new UserDTO { UserName = "username_1", Name = "name_1", Surname = "Surname_1" };
            UserDTO guest_1 = new UserDTO { UserName = "bad_username", Name = "somename", Surname = "someSurname" };
            UserDTO guest_2 = new UserDTO();

            string fullName = user.Name + " " + user.Surname;

            var mockUserService = new MockUserService()
                .GetUser();


            //Act
            HomeController homeController = new HomeController(mockUserService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(user.UserName).CustomHttpContextBase);
            ActionResult goodResult = await homeController.Index();

            //Assert
            Assert.AreEqual(fullName, HttpContextManager.Current.Session["FullName"], "Bad user full name inside Session");

            HttpContextManager.SetCurrentContext(new MockHttpContext(guest_1.UserName).CustomHttpContextBase);
            ActionResult guest1Result = await homeController.Index();

            //Assert
            Assert.ThrowsException<KeyNotFoundException>(() => HttpContextManager.Current.Session["FullName"], "Exist full name in session for guest with name");

            HttpContextManager.SetCurrentContext(new MockHttpContext(guest_2.UserName).CustomHttpContextBase);
            ActionResult guest2Result = await homeController.Index();

            //Assert
            Assert.ThrowsException<KeyNotFoundException>(() => HttpContextManager.Current.Session["FullName"], "Exist full name in session for new guest");

            Assert.IsNotNull(goodResult, "Do not return View for user.");
            Assert.IsNotNull(guest1Result, "Do not return View for guest with name.");
            Assert.IsNotNull(guest2Result, "Do not return View for new guest.");            
        }
        #endregion

        #region USER_HOME
        [TestMethod()]
        public void UserHomeTest()
        {
            //Arrange
            HomeController homeController = new HomeController(null);

            //Act
            ActionResult result = homeController.UserHome();

            //Assert
            Assert.IsNotNull(result, "Do tot return View for UserHome");
        }
        #endregion
    }
}