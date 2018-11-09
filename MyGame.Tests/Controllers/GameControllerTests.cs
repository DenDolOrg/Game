using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.BLL.DTO;
using MyGame.Controllers;
using MyGame.Infrastructure;
using MyGame.Models;
using MyGame.Real_time;
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
    public class GameControllerTests
    {
        MockGameService mockGameService;
        MockUserService mockUserService;

        [TestInitialize]
        public void Init()
        {
            ControllerDataToUse.SetData();
        }

        #region GAME_LIST
        [TestMethod()]
        public void GameListTest()
        {
            //Arrange
            string all = "all";
            string available = "available";
            string myGames = "myGames";

            string allResult = "/Game/GetAllGames";
            string availableResult = "/Game/GetAvailableGames";
            string myGamesResult = "/Game/GetUserGames";
            //Act
            GameController gameController = new GameController(null);

            var result_all = (GameActionModel)gameController.GameList(all).Model;
            var result_available = (GameActionModel)gameController.GameList(available).Model;
            var result_myGames = (GameActionModel)gameController.GameList(myGames).Model;

            //Assert
            Assert.AreEqual(allResult, result_all.ActionName, "Do not return good action name for All");
            Assert.AreEqual(availableResult, result_available.ActionName, "Do not return good action name for Available");
            Assert.AreEqual(myGamesResult, result_myGames.ActionName, "Do not return good action name for My games");
        }
        #endregion

        #region USER_GAMES
        [TestMethod()]
        public async Task GetUserGamesTest()
        {
            //Arrange
            var good_user = new UserDTO { UserName = ControllerDataToUse.UserDTO.UserName };
            var bad_user = new UserDTO { UserName = "bad_username" };

            mockGameService = new MockGameService()
                .MockGetUserGames();

            //Act
            var gameController = new GameController(null, mockGameService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(good_user.UserName).CustomHttpContextBase);
            var goodGamesRes = await gameController.GetUserGames();

            HttpContextManager.SetCurrentContext(new MockHttpContext(bad_user.UserName).CustomHttpContextBase);
            var badGamesRes = await gameController.GetUserGames();

            //Assert
            Assert.AreEqual(Json.Encode(new List<GameDTO> { ControllerDataToUse.GameDTO }), Json.Encode(goodGamesRes.Data), "Are not equal good games");
            Assert.IsNull(badGamesRes, "Not null games for user with bad bad username");
        }
        #endregion

        #region ALL_GAMES
        [TestMethod()]
        public async Task GetAllGamesTest()
        {
            //Arrange
            mockGameService = new MockGameService()
                .MockGetAllGames();

            //Act
            var gameController = new GameController(null, mockGameService.Object);
            var result = await gameController.GetAllGames();

            //Assert
            Assert.AreEqual(Json.Encode(new List<GameDTO> { ControllerDataToUse.GameDTO }), Json.Encode(result.Data));
        }
        #endregion

        #region AVAILABLE_GAMES
        [TestMethod()]
        public async Task GetAvailableGamesTest()
        {
            //Arrange

            mockGameService = new MockGameService()
                .MockGetAvailableGames();

            var good_user1 = new UserDTO { UserName = ControllerDataToUse.UserDTO.UserName };
            var bad_user = new UserDTO { UserName = "bad_username" };

            //Act
            var gameController = new GameController(null, mockGameService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(good_user1.UserName).CustomHttpContextBase);
            var result1 = await gameController.GetAvailableGames();

            HttpContextManager.SetCurrentContext(new MockHttpContext(bad_user.UserName).CustomHttpContextBase);
            var result_bad = await gameController.GetAvailableGames();

            List<GameDTO> resList_1 = Json.Decode<List<GameDTO>>(Json.Encode(result1.Data));

            //Assert
            Assert.AreEqual(resList_1.Count, 1, "Bad number of available games for user");
            Assert.IsNull(result_bad, "List of games for user with bad username is ot empty");

        }
        #endregion

        #region CREATE_GAME
        [TestMethod()]
        public async Task CreateNewgameTest()
        {
            //Arrange 
            var user_good = new UserDTO { UserName = ControllerDataToUse.UserDTO.UserName };
            var user_bad = new UserDTO { UserName = "bad_username" };

            mockGameService = new MockGameService()
                .MockCreateGame();
            mockUserService = new MockUserService()
                .MockGetUser();

            //Act
            var gameController = new GameController(mockUserService.Object, mockGameService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(user_good.UserName).CustomHttpContextBase);
            var result_good = await gameController.CreateNewGame(new NewGameModel { FirstColor = "black" });

            HttpContextManager.SetCurrentContext(new MockHttpContext(user_bad.UserName).CustomHttpContextBase);

            //Assert
            Assert.IsNotNull(result_good, "Can't create game for valid username.");
            await Assert.ThrowsExceptionAsync<HttpException>(async () => await gameController.CreateNewGame(new NewGameModel { FirstColor = "black" }), "Can create game for invalid username.");
        }
        #endregion

        #region DELETE
        [TestMethod()]
        [ExpectedException(typeof(HttpException))]
        public async Task DeleteTest()
        {
            //Arrange
            var good_game = new GameDTO { Id = ControllerDataToUse.UserDTO.Id };
            var bad_game = new GameDTO { Id = 124 };

            mockGameService = new MockGameService()
                .MockDeleteGame();

            //Act
            var gameController = new GameController(null, mockGameService.Object);
            await gameController.Delete(good_game.Id);
            await gameController.Delete(bad_game.Id);
        }
        #endregion

        #region ENTER_GAME
        [TestMethod()]
        public async Task EnterGameTest()
        {
            //Arrange 
            var game_good = ControllerDataToUse.GameDTO;
            var game_bad = new GameDTO { Id = 123 };

            UserDTO user_good;
            var user_bad = new UserDTO { UserName = "badUsername" };

            mockGameService = new MockGameService()
                .MockGetFiguresOnTable()
                .MockGetGame()
                .MockJoinGame();

            //mockUserService = new MockUserService()
            //    .MockGetUser();

            //Act
            var gameController = new GameController(null, mockGameService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(user_bad.UserName).CustomHttpContextBase);

            //Assert
            await Assert.ThrowsExceptionAsync<HttpException>(async () => await gameController.EnterGame(game_good.Id), "User with invalid username can join game with valid id.");

            //Act
            ControllerDataToUse.UserDTO.UserName = "newUsername_1";
            ControllerDataToUse.UserDTO.Id = 2;
            user_good = ControllerDataToUse.UserDTO;
            HttpContextManager.SetCurrentContext(new MockHttpContext(user_good.UserName).CustomHttpContextBase);
            var result_1_good = await gameController.EnterGame(game_good.Id);

            ControllerDataToUse.UserDTO.UserName = "newUsername_2";
            ControllerDataToUse.UserDTO.Id = 3;
            user_good = ControllerDataToUse.UserDTO;
            
            HttpContextManager.SetCurrentContext(new MockHttpContext(user_good.UserName).CustomHttpContextBase);
            //Assert
            await Assert.ThrowsExceptionAsync<HttpException>(async () => await gameController.EnterGame(game_bad.Id), "Valid user can join game with invalid id.");
            await Assert.ThrowsExceptionAsync<HttpException>(async() => await gameController.EnterGame(game_good.Id), "3 user can join game with valid id.");
            Assert.IsInstanceOfType(result_1_good, typeof(ViewResult), "Valid user can't join game with valid id.");
        }
        #endregion

        #region CHANGE_FIG_POS
        [TestMethod()]
        public async Task ChangeFieldTest()
        {
            //Arrange 
            var step_good_1 = new StepModel
            {
                FigureId = ControllerDataToUse.FigureDTO.Id.ToString(),
                NewXPos = "2",
                NewYPos = "3",
                GameId = ControllerDataToUse.GameDTO.Id.ToString(),
                FigureIdToDelete = null
            };

            var step_good_2 = new StepModel
            {
                FigureId = ControllerDataToUse.FigureDTO.Id.ToString(),
                NewXPos = "2",
                NewYPos = "3",
                GameId = ControllerDataToUse.GameDTO.Id.ToString(),
                FigureIdToDelete = ControllerDataToUse.FigureDTO.Id.ToString()
            };

            var step_bad_1 = new StepModel
            {
                FigureId = "123",
                NewXPos = "2",
                NewYPos = "3",
                GameId = ControllerDataToUse.GameDTO.Id.ToString(),
                FigureIdToDelete = null
            };

            var step_bad_2 = new StepModel
            {
                FigureId = ControllerDataToUse.FigureDTO.Id.ToString(),
                NewXPos = "2",
                NewYPos = "3",
                GameId = "123",
                FigureIdToDelete = null
            };
            var step_bad_3 = new StepModel
            {
                FigureId = ControllerDataToUse.FigureDTO.Id.ToString(),
                NewXPos = "2",
                NewYPos = "3",
                GameId = ControllerDataToUse.GameDTO.Id.ToString(),
                FigureIdToDelete = "123"
            };

            mockGameService = new MockGameService()
                .MockChangeFigurePos()
                .MockChangeTurnPriority()
                .MockDeleteFigure();

            mockUserService = new MockUserService()
                .MockGetUser();

            HttpContextManager.SetCurrentContext(new MockHttpContext(ControllerDataToUse.UserDTO.UserName).CustomHttpContextBase);
            //Act
            var gameController = new GameController(mockUserService.Object, mockGameService.Object);

            await gameController.ChangeField(step_good_1);
            await gameController.ChangeField(step_good_2);
            await Assert.ThrowsExceptionAsync<HttpException>(async () => await gameController.ChangeField(step_bad_1), "Can make changes for invalid figure id");
            await Assert.ThrowsExceptionAsync<HttpException>(async () => await gameController.ChangeField(step_bad_2), "Can make changes for invalid game id");
            await Assert.ThrowsExceptionAsync<HttpException>(async () => await gameController.ChangeField(step_bad_3), "Can make changes for invalid figure to delete id");
        }
        #endregion
    }
}