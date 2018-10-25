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
    public class PlayerManagerTests
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
                .MockPlayerProfiles();

            PlayerProfile profile = new PlayerProfile { Id = ServiceDataToUse.User.PlayerProfile.Id };
            //Act
            var playerManager = new PlayerManager(context.Object);
            var result = await playerManager.CreateAsync(profile);

            //Assert

            Assert.IsTrue(result, "Failed while creating player profile with new id.");
        }
        #endregion

        #region DELETE_ASYNC
        [TestMethod()]
        public async Task DeleteAsyncTest()
        {
            //Arrange
            var context = new MockApplicationContext()
                .MockPlayerProfiles();

            //Act
            var playerManager = new PlayerManager(context.Object);
            var result_good = await playerManager.DeleteAsync(ServiceDataToUse.User.PlayerProfile);

            //Assert
            Assert.IsTrue(result_good);
        }
        #endregion
    }
}