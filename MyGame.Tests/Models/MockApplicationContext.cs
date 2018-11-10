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

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Figure> Figures { get; set; }
        public DbSet<PlayerProfile> PlayerProfiles { get; set; }

        public MockApplicationContext():base("some")
        {
        }
        #region MOCK_PLAYER_PROFILE
        public MockApplicationContext MockPlayerProfiles()
        {
            Mock<DbSet<PlayerProfile>> mockPlayerProfiles = MockDbSet.GetDataSet(new List<PlayerProfile> { ServiceDataToUse.User.PlayerProfile });
            PlayerprofileSetup(mockPlayerProfiles);
            PlayerProfiles = mockPlayerProfiles.Object;

            Setup(m => m.PlayerProfile)
            .Returns(PlayerProfiles);
            return this;
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
        #endregion

        #region MOCK_GAMES
        public MockApplicationContext MockGames()
        {
            Mock<DbSet<Game>> mockGame = MockDbSet.GetDataSet(new List<Game> { ServiceDataToUse.Game });
            GamesSetup(mockGame);
            Games = mockGame.Object;

            Setup(m => m.Games)
            .Returns(Games);
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
        #endregion

        #region MOCK_FIGURES
        public MockApplicationContext MockFigures()
        {
            Mock<DbSet<Figure>> mockFigure = MockDbSet.GetDataSet(new List<Figure> { ServiceDataToUse.Figure });
            FiguresSetup(mockFigure);
            Figures = mockFigure.Object;

            Setup(m => m.Figures)
            .Returns(Figures);
            return this;
        }          
      
        private void FiguresSetup(Mock<DbSet<Figure>> mockFigure)
        {
            mockFigure.Setup(m => m.FindAsync(
                It.IsAny<int>()))
                .ReturnsAsync((Figure)null);

            mockFigure.Setup(m => m.FindAsync(
                It.Is<int>(id => id == ServiceDataToUse.Figure.Id)))
                .ReturnsAsync(ServiceDataToUse.Figure);
        }
        #endregion

        #region MOCK_TABLES
        public MockApplicationContext MockTables()
        {
            Mock<DbSet<Table>> mockTable = MockDbSet.GetDataSet(new List<Table> { ServiceDataToUse.Table });
            TablesSetup(mockTable);
            Tables = mockTable.Object;

            Setup(m => m.Tables)
            .Returns(Tables);
            return this;
        }

        private void TablesSetup(Mock<DbSet<Table>> mockTable)
        {
            mockTable.Setup(m => m.FindAsync(
                It.IsAny<int>()))
                .ReturnsAsync((Table)null);

            mockTable.Setup(m => m.FindAsync(
                It.Is<int>(id => id == ServiceDataToUse.Table.Id)))
                .ReturnsAsync(ServiceDataToUse.Table);
        }
        #endregion

        #region MOCK_USERS
        public MockApplicationContext MockUsers()
        {
            Mock<DbSet<ApplicationUser>> mockUser = MockDbSet.GetDataSet(new List<ApplicationUser> { ServiceDataToUse.User });
            UsersSetup(mockUser);
            Users = mockUser.Object;

            Setup(m => m.Users)
            .Returns(Users);
            return this;
        }

        private void UsersSetup(Mock<DbSet<ApplicationUser>> mockUser)
        {
            mockUser.Setup(m => m.Find(
                It.IsAny<int>()))
                .Returns((ApplicationUser)null);

            mockUser.Setup(m => m.Find(
                It.Is<int>(id => id == ServiceDataToUse.User.Id)))
                .Returns(() => ServiceDataToUse.User.Clone());
        }
        #endregion

    }
}
