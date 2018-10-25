using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.EntityFramework;
using MyGame.Tests.MockHelpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.Models
{
    internal class MockApplicationContext : Mock<ApplicationContext>
    {

        public DbSet<Game> Games { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Figure> Figures { get; set; }
        public DbSet<PlayerProfile> PlayerProfiles { get; set; }

        public MockApplicationContext():base("some")
        {
        }
        public MockApplicationContext MockPlayerProfiles()
        {
            Mock<DbSet<PlayerProfile>> mockPlayerProfiles = MockDbSet.GetDataSet(new List<PlayerProfile> { ServiceDataToUse.User.PlayerProfile });
            PlayerprofileSetup(mockPlayerProfiles);
            PlayerProfiles = mockPlayerProfiles.Object;

            Setup(m => m.PlayerProfile)
            .Returns(PlayerProfiles);
            return this;
        }

        public MockApplicationContext MockGames()
        {
            Mock<DbSet<Game>> mockGame = MockDbSet.GetDataSet(new List<Game> { ServiceDataToUse.Game });
            GamesSetup(mockGame);
            Games = mockGame.Object;

            Setup(m => m.Games)
            .Returns(Games);
            return this;
        }

        public MockApplicationContext MockFigures()
        {
            Mock<DbSet<Figure>> mockFigure = MockDbSet.GetDataSet(new List<Figure> { ServiceDataToUse.Figure });
            Figures = mockFigure.Object;

            Setup(m => m.Figures)
            .Returns(Figures);
            return this;
        }          
        private void GamesSetup(Mock<DbSet<Game>> mockGame)
        {
            mockGame.Setup(m => m.FindAsync(
                It.IsAny<int>()))
                .ReturnsAsync((Game)null);

            mockGame.Setup(m => m.FindAsync(
                It.Is<int>(id => id == ServiceDataToUse.Game.Id)))
                .ReturnsAsync(ServiceDataToUse.Game);
        }

        private void PlayerprofileSetup(Mock<DbSet<PlayerProfile>> mocktable)
        {
            mocktable.Setup(m => m.FindAsync(
                It.IsAny<int>()))
                .ReturnsAsync((PlayerProfile)null);

            mocktable.Setup(m => m.FindAsync(
                It.Is<int>(id => id == ServiceDataToUse.User.PlayerProfile.Id)))
                .ReturnsAsync(ServiceDataToUse.User.PlayerProfile);
        }
    }
}
