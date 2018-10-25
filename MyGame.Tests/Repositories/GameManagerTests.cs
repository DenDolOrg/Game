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
            var tableManager = new GameManager(context.Object);
            var result = await tableManager.DeleteAsync(game);

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

            Table table = ServiceDataToUse.Table;

            //Act
            var tableManager = new GameManager(context.Object);
            var result_good = await tableManager.FindByIdAsync(ServiceDataToUse.Table.Id);
            var result_bad = await tableManager.FindByIdAsync(123);

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

            Table table = ServiceDataToUse.Table;

            //Act
            var tableManager = new GameManager(context.Object);
            var result = tableManager.GetAllGames();

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
    }
}