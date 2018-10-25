﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.BLL.DTO;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.Tests.MockHelpers;
using MyGame.Tests.Models;

namespace MyGame.Tests.MockManagers
{
    internal class MockGameManager : Mock<IGameManager>
    {
        public MockGameManager MockCreateAsync()
        {

            Setup(m => m.CreateAsync(
                It.IsAny<Game>()))
                .ReturnsAsync(true).Callback((Game t) => t.Id = 2);
            return this;
        }

        public MockGameManager MockDeleteAsync()
        {
            Setup(m => m.DeleteAsync(
                It.IsAny<Game>()
                )).ReturnsAsync(false);

            Setup(m => m.DeleteAsync(
                It.Is<Game>(t => t.Id == ServiceDataToUse.Game.Id)))
                .ReturnsAsync(true);
            return this;
        }

        public MockGameManager MockFindByIdAsync()
        {
            Setup(m => m.FindByIdAsync(
                It.IsAny<int>()
                )).ReturnsAsync((Game)null);

            Setup(m => m.FindByIdAsync(
                It.Is<int>(id => id == ServiceDataToUse.Game.Id)))
                .ReturnsAsync(ServiceDataToUse.Game);
            return this;
        }

        public MockGameManager MockGetAllGames()
        {
            Setup(m => m.GetAllGames())
                .Returns(GetDbSetGames(new List <Game> { ServiceDataToUse.Game }));
            return this;
        }

        public MockGameManager MockGetUserGames()
        {
            Setup(m => m.GetGamesForUser(
                It.IsAny<int>()))
                .Returns(() => GetDbSetGames(null));
     

            Setup(m => m.GetGamesForUser(
                It.Is<int>(id => id == ServiceDataToUse.User.Id)))
                .Returns(GetDbSetGames(new List<Game> { ServiceDataToUse.Game }));
            return this;
        }

        public MockGameManager MockGetAvailableGames()
        {
            Setup(m => m.GetAvailableGames(
                It.IsAny<int>()))
                .Returns(() => GetDbSetGames(null));


            Setup(m => m.GetAvailableGames(
                It.Is<int>(id => id == ServiceDataToUse.User.Id)))
                .Returns(GetDbSetGames(new List<Game> { ServiceDataToUse.Game }));
            return this;
        }

        #region HELPERS

        private IQueryable<Game> GetDbSetGames(List<Game> tables)
        {
            return MockDbSet.GetDataSet(tables).Object;
        }
        #endregion

    }
}
