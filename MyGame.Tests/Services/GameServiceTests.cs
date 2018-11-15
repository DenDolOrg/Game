using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.BLL.DTO;
using MyGame.BLL.Infrastructure;
using MyGame.BLL.Services;
using MyGame.Tests.MockManagers;
using MyGame.Tests.Models;
using MyGame.Tests.MockStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.Services.Tests
{
    [TestClass()]
    public class GameServiceTests
    {
        MockUnitOfWork Work;
        MockUserManager userManager;
        MockTableManager tableManager;
        MockGameManager gameManager;
        MockFigureManager figureManager;
        [TestInitialize]
        public void Init()
        {
            Work = new MockUnitOfWork();
            ServiceDataToUse.SetData();
        }
        #region CREATE_GAME
        [TestMethod()]
        public async Task CreateNewGameTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByNameAsync();

            gameManager = new MockGameManager()
                .MockCreateAsync();

            tableManager = new MockTableManager()
               .MockCreateAsync();

            figureManager = new MockFigureManager()
                .MockCreateAsync();

            Work.SetManagers(userManager, gameManager, tableManager, figureManager);

            var good_game = new GameDTO { Opponents = new List<UserDTO> { new UserDTO { UserName = ServiceDataToUse.User.UserName } } };
            var bad_game = new GameDTO { Opponents = new List<UserDTO> { new UserDTO { UserName = "bad_username" } } };

            //Act
            var service = new GameService(Work.Object);
            var userDTO_good = await service.CreateNewGame(good_game);
            var userDTO_bad = await service.CreateNewGame(bad_game);

            //Assert
            Assert.IsNotNull(userDTO_good, "Failed while creating new table.");
            Assert.IsNull(userDTO_bad, "Succes while creating new table for user with bad username.");
        }
        #endregion

        #region DETELE_TABLE
        [TestMethod()]
        public async Task DeteteGameTest()
        {
            //Arrang
            gameManager = new MockGameManager()
                .MockDeleteAsync()
                .MockFindByIdAsync();

            tableManager = new MockTableManager()
                .MockDeleteAsync();

            figureManager = new MockFigureManager()
                .MockDeleteAsync();

            Work.SetManagers(null, gameManager, tableManager, figureManager);

            var tableDTO_good = new GameDTO { Id = ServiceDataToUse.Table.Id };
            var tableDTO_bad = new GameDTO { Id = 124 };
            //Act
            GameService service = new GameService(Work.Object);
            var details_good = await service.DeteteGame(tableDTO_good);
            var details_bad = await service.DeteteGame(tableDTO_bad);

            //Assert
            Assert.IsTrue(details_good.Succedeed, "Failed while deleting new table.");
            Assert.IsFalse(details_bad.Succedeed, "Succes while deleting bad table.");
        }
        #endregion

        #region DETELE_USER_TABLES
        [TestMethod()]
        public async Task DeteteUserGamesTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByIdAsync();

            gameManager = new MockGameManager()
                .MockDeleteAsync()
                .MockFindByIdAsync();

            figureManager = new MockFigureManager()
               .MockDeleteAsync();

            Work.SetManagers(userManager, gameManager, null, figureManager);

            var user_good = new UserDTO { Id = ServiceDataToUse.User.Id };
            var user_bad = new UserDTO { Id = 123 };

            //Act
            var service = new GameService(Work.Object);
            var details_good = await service.DeteteUserGame(user_good);
            var details_bad = await service.DeteteUserGame(user_bad);

            //Assert
            Assert.IsTrue(details_good.Succedeed, "Failed while deleting good user tables.");
            Assert.IsFalse(details_bad.Succedeed, "Succes while deleting bad user tables.");
        }
        #endregion

        #region GET_FIGURES_ON_TABLE
        [TestMethod()]
        public async Task GetFiguresOnTableTest()
        {
            //Arrange
            figureManager = new MockFigureManager()
                .MockGetFiguresForTable();

            Work.SetManagers(null, null, null, figureManager);

            var table_good = new GameDTO { Id = ServiceDataToUse.User.Id };
            var table_bad = new GameDTO { Id = 123 };

            //Act
            var service = new GameService(Work.Object);
            var result_good = await service.GetFiguresOnTable(table_good);
            var result_bad = await service.GetFiguresOnTable(table_bad);

            //Assert
            Assert.AreEqual(result_good.Count(), 1, "Bad number of figures on the fresh table");
            Assert.IsNull(result_bad, "Not 0 figures on table with bad Id");
        }
        #endregion

        #region GET_TABLE
        [TestMethod()]
        public async Task GetGameTest()
        {
            //Arrange
            gameManager = new MockGameManager()
                .MockFindByIdAsync();

            Work.SetManagers(null, gameManager);

            var table_good = new GameDTO { Id = 1 };
            var table_bad = new GameDTO { Id = 123 };

            //Act
            var service = new GameService(Work.Object);

            var result_good = await service.GetGame(table_good);
            var result_bad = await service.GetGame(table_bad);

            //Assert
            Assert.AreEqual(result_good.Id, table_good.Id, "Not the same id returned for good table.");
            Assert.IsNotNull(result_good.Opponents, "No opponents for good table");
            Assert.IsNull(result_bad, "Not null table with bad table Id");
        }
        #endregion

        #region GET_ALL_TABLES
        [TestMethod()]
        public async Task GetAllGamesTest()
        {
            //Arrange
            gameManager = new MockGameManager()
                .MockGetAllGames();

            Work.SetManagers(null, gameManager);

            //Act
            GameService service = new GameService(Work.Object);
            var result = await service.GetAllGames();

            //Assert
            Assert.AreEqual(result.Count(), 1);
        }
        #endregion

        #region GET_USER_TABLES
        [TestMethod()]
        public async Task GetUserGamesTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByNameAsync();

            gameManager = new MockGameManager()
                .MockGetUserGames();

            Work.SetManagers(userManager, gameManager);

            var user_good = new UserDTO { UserName = ServiceDataToUse.User.UserName };
            var user_bad = new UserDTO { UserName = "bad_username" };

            //Act
            var service = new GameService(Work.Object);
            var result_good = await service.GetUserGames(user_good);
            var result_bad = await service.GetUserGames(user_bad);

            //Assert
            Assert.AreEqual(result_good.Count(), 1, "Bad number of available tables.");
            Assert.IsNull(result_bad, "Not 0 available tables for bad user.");
        }
        #endregion

        #region GET_AVAILABLE_TABLES
        [TestMethod()]
        public async Task GetAvailableGamesTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByNameAsync();

            gameManager = new MockGameManager()
                .MockGetAvailableGames();

            Work.SetManagers(userManager, gameManager);

            var user_good = new UserDTO { UserName = ServiceDataToUse.User.UserName };
            var user_bad = new UserDTO { UserName = "bad_username" };

            //Act
            var service = new GameService(Work.Object);
            var result_good = await service.GetAvailableGames(user_good);
            var result_bad = await service.GetAvailableGames(user_bad);

            //Assert
            Assert.AreEqual(result_good.Count(), 1);
            Assert.IsNull(result_bad);
        }
        #endregion

        #region CHANGE_FIGURE_POSITION
        [TestMethod()]
        public async Task ChangeFigurePosTest()
        {
            //Arrange
            figureManager = new MockFigureManager()
                .MockFindByIdAsync();

            Work.SetManagers(null, null, null, figureManager);

            var figure_good = new FigureDTO { Id = ServiceDataToUse.Figure.Id, XCoord = 2, YCoord = 2 };
            var figure_bad = new FigureDTO { Id = 123 };

            //Act
            GameService service = new GameService(Work.Object);
            var result_good = await service.ChangeFigurePos(figure_good);
            var result_bad = await service.ChangeFigurePos(figure_bad);

            //Assert
            Assert.IsTrue(result_good.Succedeed, "Failed while changing position for valid figure.");
            Assert.AreEqual(ServiceDataToUse.Figure.X, 2, "Bad X coord.");
            Assert.AreEqual(ServiceDataToUse.Figure.Y, 2, "Bad Y coord.");
            Assert.IsFalse(result_bad.Succedeed, "Succes while changing position for invalid figure.");
        }
        #endregion

        #region JOIN_GAME
        [TestMethod()]
        public async Task JoinGameTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByNameAsync();

            gameManager = new MockGameManager()
                .MockFindByIdAsync()
                .MockAddOpponentToGame();

            Work.SetManagers(userManager, gameManager);

            var user_good = new UserDTO { UserName = ServiceDataToUse.User.UserName };
            var user_bad = new UserDTO { UserName = "bad_username" };

            var game_good = new GameDTO { Id = ServiceDataToUse.Game.Id };
            var game_bad = new GameDTO { Id = 123 };

            ServiceDataToUse.Game.Opponents.Clear();
            //Act
            var service = new GameService(Work.Object);

            var result_good_1 = await service.JoinGame(user_good, game_good);
            var result_bad_1 = await service.JoinGame(user_bad, game_good);
            var result_bad_2 = await service.JoinGame(user_good, game_bad);
            var result_good_3 = await service.JoinGame(user_good, game_good);

            ServiceDataToUse.User.Id = 2;
            var result_good_2 = await service.JoinGame(user_good, game_good);

            //Assert
            Assert.IsTrue(result_good_1.Succedeed, "Error while adding valid user to valid table.");
            Assert.IsTrue(result_good_2.Succedeed, "Error while adding second valid user to valid table.");
            Assert.IsTrue(result_good_3.Succedeed, "Error while adding same valid user to valid table.");
            Assert.AreEqual(ServiceDataToUse.Game.Opponents.Count, 2);
            Assert.IsFalse(result_bad_1.Succedeed, "Succes while adding invalid user to valid table.");
            Assert.IsFalse(result_bad_2.Succedeed, "Succes while adding valid user to invalid table.");
        }
        #endregion

        [TestMethod()]
        public async Task ChangeTurnPriorityTest()
        {
            //Arrange
            gameManager = new MockGameManager()
                .MockFindByIdAsync()
                .MockTurnChange();

            Work.SetManagers(null, gameManager);

            var table_good = new GameDTO { Id = ServiceDataToUse.Game.Id, LastTurnPlayerId = 2 };
            var table_bad = new GameDTO { Id = 123, LastTurnPlayerId = 2 };

            //Act
            var service = new GameService(Work.Object);

            var result_good = await service.ChangeTurnPriority(table_good);
            var result_bad = await service.ChangeTurnPriority(table_bad);

            //Assert
            Assert.IsTrue(result_good.Succedeed, "Error while changing turn priority for valid game");
            Assert.IsFalse(result_bad.Succedeed, "Success while changing turn priority for invalid game");
        }

        [TestMethod()]
        public async Task DeleteFiguresTest()
        {
            //Arrange

            figureManager = new MockFigureManager()
                .MockDeleteSomeFiguresAsync();

            Work.SetManagers(null, null, null, figureManager);

            var figure_good = new FigureDTO { Id = ServiceDataToUse.Figure.Id };
            var figure_bad = new FigureDTO { Id = 123 };

            //Act
            var service = new GameService(Work.Object);

            var result_good = await service.DeleteFigures(new List<FigureDTO> { figure_good });
            var result_bad = await service.DeleteFigures(new List<FigureDTO> { figure_bad });

            //Assert
            Assert.IsTrue(result_good.Succedeed, "Error while delting figure with valid id");
            Assert.IsFalse(result_bad.Succedeed, "Success while deleting figure with invalid id");
        }

    }
}