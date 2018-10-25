using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.BLL.DTO;
using MyGame.Controllers;
using MyGame.Infrastructure;
using MyGame.Models;
using MyGame.Tests.MockHelpers;
using MyGame.Tests.Models;
using MyGame.Tests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MyGame.Controllers.Tests
{
    [TestClass()]
    public class TableControllerTests
    {

        [TestInitialize]
        public void Init()
        {
            ControllerDataToUse.SetData();
        }

        #region TABLE_LIST
        [TestMethod()]
        public void TableListTest()
        {
            //Arrange
            string all = "all";
            string available = "available";
            string myGames = "myGames";

            string allResult = "/Game/GetAllGames";
            string availableResult = "/Game/GetAvailableGames";
            string myGamesResult = "/Game/GetUserGames";
            //Act
            GameController tableController = new GameController(null);

            TableActionModel result_all = (TableActionModel)tableController.GameList(all).Model;
            TableActionModel result_available = (TableActionModel)tableController.GameList(available).Model;
            TableActionModel result_myGames = (TableActionModel)tableController.GameList(myGames).Model;

            //Assert
            Assert.AreEqual(allResult, result_all.ActionName, "Do not return good action name for All");
            Assert.AreEqual(availableResult, result_available.ActionName, "Do not return good action name for Available");
            Assert.AreEqual(myGamesResult, result_myGames.ActionName, "Do not return good action name for My tables");
        }
        #endregion

        #region USER_TABLES
        [TestMethod()]
        public async Task GetUserGamesTest()
        {
            //Arrange
            UserDTO good_user = new UserDTO { UserName = ControllerDataToUse.UserDTO.UserName };
            UserDTO bad_user = new UserDTO { UserName = "bad_username" };

            var mockGameService = new MockGameService()
                .MockGetUserGames();

            //Act
            GameController tableController = new GameController(null, mockGameService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(good_user.UserName).CustomHttpContextBase);
            var goodGamesRes = await tableController.GetUserGames();

            HttpContextManager.SetCurrentContext(new MockHttpContext(bad_user.UserName).CustomHttpContextBase);
            var badGamesRes = await tableController.GetUserGames();

            //Assert
            Assert.AreEqual(Json.Encode(new List<GameDTO> { ControllerDataToUse.GameDTO }), Json.Encode(goodGamesRes.Data), "Are not equal good tables");
            Assert.IsNull(badGamesRes, "Not null tables for user with bad bad username");
        }
        #endregion

        #region ALL_TABLES
        [TestMethod()]
        public async Task GetAllGamesTest()
        {
            //Arrange
            var mockGameService = new MockGameService()
                .MockGetAllGames();

            //Act
            GameController tableController = new GameController(null, mockGameService.Object);
            var result = await tableController.GetAllGames();

            //Assert
            Assert.AreEqual(Json.Encode(new List<GameDTO> { ControllerDataToUse.GameDTO }), Json.Encode(result.Data));
        }
        #endregion

        #region AVAILABLE_TABLES
        [TestMethod()]
        public async Task GetAvailableGamesTest()
        {
            //Arrange

            var mockGameService = new MockGameService()
                .MockGetAvailableGames();

            UserDTO good_user1 = new UserDTO { UserName = ControllerDataToUse.UserDTO.UserName };
            UserDTO bad_user = new UserDTO { UserName = "bad_username" };

            //Act
            GameController tableController = new GameController(null, mockGameService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(good_user1.UserName).CustomHttpContextBase);
            var result1 = await tableController.GetAvailableGames();

            HttpContextManager.SetCurrentContext(new MockHttpContext(bad_user.UserName).CustomHttpContextBase);
            var result_bad = await tableController.GetAvailableGames();

            List<GameDTO> resList_1 = Json.Decode<List<GameDTO>>(Json.Encode(result1.Data));

            //Assert
            Assert.AreEqual(resList_1.Count, 1, "Bad number of available tables for user");
            Assert.IsNull(result_bad, "List of tables for user with bad username is ot empty");

        }
        #endregion

        #region CREATE_TABLE
        [TestMethod()]
        public async Task CreateNewtableTest()
        {
            //Arrange 
            UserDTO user_good = new UserDTO { UserName = ControllerDataToUse.UserDTO.UserName };
            UserDTO user_bad = new UserDTO { UserName = "bad_username" };

            var mockGameService = new MockGameService()
                .MockCreateGame();
            
            //Act
            GameController tableController = new GameController(null, mockGameService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(user_good.UserName).CustomHttpContextBase);
            var result_good = await tableController.CreateNewGame();

            HttpContextManager.SetCurrentContext(new MockHttpContext(user_bad.UserName).CustomHttpContextBase);
            var result_bad = await tableController.CreateNewGame();

            //Assert
            Assert.IsNotNull(result_good, "Can't create table for valid username.");
            Assert.IsNull(result_bad, "Can create table for invalid username.");
        }
        #endregion

        #region DELETE
        [TestMethod()]
        [ExpectedException(typeof(HttpException))]
        public async Task DeleteTest()
        {
            //Arrange
            GameDTO good_table = new GameDTO { Id = ControllerDataToUse.UserDTO.Id };
            GameDTO bad_table = new GameDTO { Id = 124 };

            var mockGameService = new MockGameService()
                .MockDeleteGame();

            //Act
            GameController tableController = new GameController(null, mockGameService.Object);
            await tableController.Delete(good_table.Id);
            await tableController.Delete(bad_table.Id);
        }
        #endregion
    }
}