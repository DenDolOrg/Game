using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.DAL.Entities;
using MyGame.DAL.Repositories;
using MyGame.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Repositories.Tests
{
    [TestClass()]
    public class GameManagerTests
    {
        [TestInitialize]
        public void Init()
        {
            ServiceDataToUse.SetData();
        }

        #region CREATE_ASYNC
        [TestMethod()]
        public async Task CreateAsyncTest()
        {
            //Arrange
            var context = new MockApplicationContext()
                .MockGames();

            Game game = ServiceDataToUse.Game;

            //Act
            var gameManager = new GameManager(context.Object);
            var result = await gameManager.CreateAsync(game);

            //Assert
            Assert.IsTrue(result, "Failed while creating table with new id.");
        }
        #endregion

        #region DELETE_ASYNC
        [TestMethod()]
        public async Task DeleteAsyncTest()
        {
            //Arrange
            var context = new MockApplicationContext()
                .MockGames();

            Game game = ServiceDataToUse.Game;

            //Act
            var gameManager = new GameManager(context.Object);
            var result = await gameManager.DeleteAsync(game);

            //Assert
            Assert.IsTrue(result, "Failed while deleting table with new id.");
        }
        #endregion

        #region FIND_BY_ID
        [TestMethod()]
        public async Task FindByIdTest()
        {
            //Arrange
            var context = new MockApplicationContext()
                .MockGames();

            //Act
            var gameManager = new GameManager(context.Object);
            var result_good = await gameManager.FindByIdAsync(ServiceDataToUse.Table.Id);
            var result_bad = await gameManager.FindByIdAsync(123);

            //Assert
            Assert.IsNotNull(result_good, "Failed finding table with id.");
            Assert.IsNull(result_bad, "Succed finding table with bad id.");
        }
        #endregion

        #region GET_ALL_GAMES
        [TestMethod()]
        public void GetAllGamesTest()
        {
            //Arrange
            var context = new MockApplicationContext()
                .MockGames();

            //Act
            var gameManager = new GameManager(context.Object);
            var result = gameManager.GetAllGames();

            //Assert
            Assert.AreEqual(result.Count(), 1, "Failed getting all tables.");
        }
        #endregion

        #region GET_TABLES_FOR_USER
        [TestMethod()]
        public void GetGamesForUserTest()
        {
            //Arrange
            var context = new MockApplicationContext()
                .MockGames();

            Table table = ServiceDataToUse.Table;

            //Act
            var tableManager = new GameManager(context.Object);
            var result = tableManager.GetGamesForUser(ServiceDataToUse.User.Id);

            //Assert
            Assert.AreEqual(result.Count(), 1, "Failed getting user tables.");
        }
        #endregion

        #region GET_AVAILABLE_TABLES
        [TestMethod()]
        public void GetAvailableGamesTest()
        {
            //Arrange
            var context = new MockApplicationContext()
                .MockGames();

            Table table = ServiceDataToUse.Table;

            //Act
            var tableManager = new GameManager(context.Object);
            var result = tableManager.GetAvailableGames(ServiceDataToUse.User.Id);

            //Assert
            Assert.AreEqual(result.Count(), 0, "Failed getting available tables.");
        }
        #endregion

        #region ADD_OPPONENT
        [TestMethod()]
        public async Task AddOpponentToGameTest()
        {
            //Arrange
            var context = new MockApplicationContext()
                .MockGames()
                .MockUsers();

            ServiceDataToUse.Game.Opponents.Clear();
            //Act
            var gameManager = new GameManager(context.Object);
            var result_good_1 = await gameManager.AddOpponentToGame(ServiceDataToUse.Game.Id, ServiceDataToUse.User.Id);

            ServiceDataToUse.User.Id = 3;
            var result_good_2 = await gameManager.AddOpponentToGame(ServiceDataToUse.Game.Id, ServiceDataToUse.User.Id);

            var result_good_3 = await gameManager.AddOpponentToGame(ServiceDataToUse.Game.Id, ServiceDataToUse.User.Id);

            var result_bad_1 = await gameManager.AddOpponentToGame(123, ServiceDataToUse.User.Id);
            var result_bad_2 = await gameManager.AddOpponentToGame(ServiceDataToUse.Game.Id, 123);

            //Assert
            Assert.IsNotNull(result_good_1, "Failed while adding valid user to valid game.");
            Assert.IsNotNull(result_good_2, "Failed while adding second valid user to valid game.");
            Assert.IsNotNull(result_good_3, "Failed while adding same user to valid game.");
            Assert.AreEqual(ServiceDataToUse.Game.Opponents.Count, 2, "Not valid number of opponents.");
            Assert.IsNull(result_bad_1, "Succes while adding invalid user to valid game.");
            Assert.IsNull(result_bad_2, "Succes while adding valid user to invalid game.");
        }
        #endregion
    }
}