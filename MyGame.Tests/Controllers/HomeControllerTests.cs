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
using MyGame.Tests.Models;

namespace MyGame.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {

        [TestInitialize]
        public void Init()
        {
            ControllerDataToUse.SetData();
        }

        #region INDEX
        [TestMethod()]
        public async Task IndexTest()
        {
            //Arrange
            UserDTO guest_1 = new UserDTO { UserName = "bad_username", Name = "somename", Surname = "someSurname" };
            UserDTO guest_2 = new UserDTO();

            string fullName = ControllerDataToUse.UserDTO.Name + " " + ControllerDataToUse.UserDTO.Surname;

            var mockUserService = new MockUserService()
                .MockGetUser();


            //Act_1
            HomeController homeController = new HomeController(mockUserService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(ControllerDataToUse.UserDTO.UserName).CustomHttpContextBase);
            ActionResult goodResult = await homeController.Index();

            //Assert_1
            Assert.AreEqual(fullName, HttpContextManager.Current.Session["FullName"], "Bad user full name inside Session");
            
            //Act_2
            HttpContextManager.SetCurrentContext(new MockHttpContext(guest_1.UserName).CustomHttpContextBase);
            ActionResult guest1Result = await homeController.Index();

            //Assert_2
            Assert.ThrowsException<KeyNotFoundException>(() => HttpContextManager.Current.Session["FullName"], "Exist full name in session for guest with name");

            //Act_3

            HttpContextManager.SetCurrentContext(new MockHttpContext(guest_2.UserName).CustomHttpContextBase);
            HttpContextManager.Current.Session["FullName"] = fullName;
            ActionResult logoutResult = await homeController.Index();

            //Assert_3
            Assert.ThrowsException<KeyNotFoundException>(() => HttpContextManager.Current.Session["FullName"], "Exist full name in session for guest who loged out");

            //Act_4
            HttpContextManager.SetCurrentContext(new MockHttpContext(guest_2.UserName).CustomHttpContextBase);
            ActionResult guest2Result = await homeController.Index();

            //Assert_4
            Assert.ThrowsException<KeyNotFoundException>(() => HttpContextManager.Current.Session["FullName"], "Exist full name in session for new guest");

            //Assert_final
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