using System;
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
                .ReturnsAsync(true).Callback((Game t) => { t.Id = 2; t.Table = ServiceDataToUse.Table; });
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

        public MockGameManager MockAddOpponentToGame()
        {
            Setup(m => m.AddOpponentToGame(
                It.IsAny<int>(),
                It.IsAny<ApplicationUser>()))
                .ReturnsAsync((Game)null);

            Setup(m => m.AddOpponentToGame(
                It.Is<int>(gId => gId == ServiceDataToUse.Game.Id),
                It.Is<ApplicationUser>(u => (u.Id == ServiceDataToUse.User.Id) &&
                                  (ServiceDataToUse.Game.Opponents.Count != 2))))
                .ReturnsAsync(ServiceDataToUse.Game).Callback<int, ApplicationUser>((gId, u) =>
                {
                    if(!ServiceDataToUse.Game.Opponents.Select(o => o.Id).Contains(u.Id))
                        ServiceDataToUse.Game.Opponents.Add(ServiceDataToUse.User.Clone());
                });

            return this;
        }

        public MockGameManager MockTurnChange()
        {
            Setup(m => m.TurnChange(
                It.IsAny<int>(),
                It.IsAny<int>()))
                .ReturnsAsync(false);

            Setup(m => m.TurnChange(
                It.Is<int>(g => g == ServiceDataToUse.Game.Id),
                It.IsAny<int>()))
                .ReturnsAsync(true);

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
