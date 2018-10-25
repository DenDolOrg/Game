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

            UserDTO good_user = new UserDTO { UserName = ServiceDataToUse.User.UserName };
            UserDTO bad_user = new UserDTO { UserName = "bad_username" };

            //Act
            GameService service = new GameService(Work.Object);
            OperationDetails details_good = await service.CreateNewGame(good_user);
            OperationDetails details_bad = await service.CreateNewGame(bad_user);

            //Assert
            Assert.IsTrue(details_good.Succedeed, "Failed while creating new table.");
            Assert.IsFalse(details_bad.Succedeed, "Succes while creating new table for user with bad username.");
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

            GameDTO tableDTO_good = new GameDTO { Id = ServiceDataToUse.Table.Id };
            GameDTO tableDTO_bad = new GameDTO { Id = 124 };
            //Act
            GameService service = new GameService(Work.Object);
            OperationDetails details_good = await service.DeteteGame(tableDTO_good);
            OperationDetails details_bad = await service.DeteteGame(tableDTO_bad);

            //Assert
            Assert.IsTrue(details_good.Succedeed, "Failed while deleting new table.");
            Assert.IsFalse(details_bad.Succedeed, "Succes while deleting bad table.");
        }
        #endregion

        #region DETELE_USER_TABLES
        [TestMethod()]
        public async Task DeteteUserTablesTest()
        {
            //Arrange
            userManager = new MockUserManager(new MockUserStore().Object)
                .MockFindByIdAsync();

            gameManager = new MockGameManager()
                .MockDeleteAsync()
                .MockFindByIdAsync();

            figureManager = new MockFigureManager()
               .MockDeleteAsync();

            Work.SetManagers(userManager, gameManager,null, figureManager);

            UserDTO user_good = new UserDTO { Id = ServiceDataToUse.User.Id };
            UserDTO user_bad = new UserDTO { Id = 123};

            //Act
            GameService service = new GameService(Work.Object);
            OperationDetails details_good = await service.DeteteUserGame(user_good);
            OperationDetails details_bad = await service.DeteteUserGame(user_bad);

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

            GameDTO table_good = new GameDTO { Id = ServiceDataToUse.User.Id };
            GameDTO table_bad = new GameDTO { Id = 123 };
            
            //Act
            GameService service = new GameService(Work.Object);
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

            GameDTO table_good = new GameDTO{ Id = 1 };
            GameDTO table_bad = new GameDTO{ Id = 123 };
  
            //Act
            GameService service = new GameService(Work.Object);

            GameDTO result_good = await service.GetGame(table_good);
            GameDTO result_bad = await service.GetGame(table_bad);

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

            UserDTO user_good = new UserDTO { UserName = ServiceDataToUse.User.UserName};
            UserDTO user_bad = new UserDTO { UserName = "bad_username"};

            //Act
            GameService service = new GameService(Work.Object);
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

            UserDTO user_good = new UserDTO { UserName = ServiceDataToUse.User.UserName };
            UserDTO user_bad = new UserDTO { UserName = "bad_username" };

            //Act
            GameService service = new GameService(Work.Object);
            var result_good = await service.GetAvailableGames(user_good);
            var result_bad = await service.GetAvailableGames(user_bad);

            //Assert
            Assert.AreEqual(result_good.Count(), 1);
            Assert.IsNull(result_bad);
        }
        #endregion
    }
}