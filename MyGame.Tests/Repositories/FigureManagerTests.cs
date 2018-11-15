using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.DAL.Entities;
using MyGame.DAL.Repositories;
using MyGame.Tests.MockHelpers;
using MyGame.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Repositories.Tests
{
    [TestClass()]
    public class FigureManagerTests
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
            MockApplicationContext context = new MockApplicationContext()
                .MockGames()
                .MockTables()
                .MockFigures();
            //Act
            FigureManager manager = new FigureManager(context.Object);
            var result_good = await manager.CreateAsync(ServiceDataToUse.Table.Id);
            var result_bad = await manager.CreateAsync(123);

            //Assert
            Assert.IsTrue(result_good, "Failed while creating figures for valid table.");
            Assert.IsFalse(result_bad, "Can create figures for invalid table.");
        }
        #endregion

        #region DELETE_ASYNC
        [TestMethod()]
        public async Task DeleteAsyncTest()
        {
            //Arrange
            MockApplicationContext context = new MockApplicationContext()
                .MockGames()
                .MockFigures();

            //Act
            FigureManager manager = new FigureManager(context.Object);
            var result_good = await manager.DeleteAsync(ServiceDataToUse.Table.Id);

            //Assert
            Assert.IsTrue(result_good, "Failed while deleting figures for valid table.");
        }
        #endregion

        #region GET_FIGURES_FOR_TABLE
        [TestMethod()]
        public void GetFiguresForTableTest()
        {
            //Arrange
            MockApplicationContext context = new MockApplicationContext()
                .MockFigures();

            //Act
            FigureManager manager = new FigureManager(context.Object);
            var result_good = manager.GetFiguresForTable(ServiceDataToUse.Table.Id);

            //Assert
            Assert.AreEqual(result_good.Count(), 1, "Failed while taking figures for valid table.");
        }
        #endregion

        [TestMethod()]
        public async Task FindByIdAsyncTest()
        {
            //Arrange
            MockApplicationContext context = new MockApplicationContext()
                .MockFigures();
            //Act
            FigureManager manager = new FigureManager(context.Object);
            var result_good = await manager.FindByIdAsync(ServiceDataToUse.Figure.Id);
            var result_bad = await manager.FindByIdAsync(123);

            //Assert
            Assert.IsNotNull(result_good, "Failed while getting figure with valid id.");
            Assert.IsNull(result_bad, "Can get figure with invalid id.");
        }

        [TestMethod()]
        public async Task DeleteSingleFigureAsyncTest()
        {
            //Arrange
            MockApplicationContext context = new MockApplicationContext()
                .MockFigures();
            //Act
            FigureManager manager = new FigureManager(context.Object);
            var result_good = await manager.DeleteSomeFiguresAsync(new List<int> { ServiceDataToUse.Figure.Id });
            var result_bad = await manager.DeleteSomeFiguresAsync(new List<int> { 123 });

            //Assert
            Assert.IsTrue(result_good, "Failed while deleting figure with valid id.");
            Assert.IsFalse(result_bad, "Can delete figure with invalid id.");
        }
    }
}