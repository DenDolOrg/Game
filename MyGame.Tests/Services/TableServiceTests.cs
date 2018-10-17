using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.BLL.DTO;
using MyGame.BLL.Infrastructure;
using MyGame.BLL.Services;
using MyGame.Tests.MockEnity;
using MyGame.Tests.MockManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.Services.Tests
{
    [TestClass()]
    public class TableServiceTests
    {

        #region CREATE_TABLE
        [TestMethod()]
        public async Task CreateNewTableTest()
        {
            //Arrange
            MockUnitOfWork work = new MockUnitOfWork();

            MockUserManager userManager = new MockUserManager(work.GetUserStore().Object)
                .MockFindByNameAsync();

            MockTableManager tableManager = new MockTableManager()
                .MockCreateAsync();

            MockFigureManager figureManager = new MockFigureManager()
                .MockCreateAsync();

            work.SetManagers(userManager, tableManager, figureManager);

            UserDTO good_user = new UserDTO { UserName = "username_1" };
            UserDTO bad_user = new UserDTO { UserName = "bad_username" };

            //Act
            TableService service = new TableService(work.Object);
            OperationDetails details_good = await service.CreateNewTable(good_user);
            OperationDetails details_bad = await service.CreateNewTable(bad_user);

            //Assert
            Assert.IsTrue(details_good.Succedeed, "Failed while creating new table.");
            Assert.IsFalse(details_bad.Succedeed, "Succes while creating new table for user with bad username.");
        }
        #endregion

        #region DETELE_TABLE
        [TestMethod()]
        public async Task DeteteTableTest()
        {
            //Arrange
            MockUnitOfWork work = new MockUnitOfWork();

            MockTableManager tableManager = new MockTableManager()
                .MockDeleteAsync()
                .MockFindByIdAsync();

            MockFigureManager figureManager = new MockFigureManager()
                .MockDeleteAsync();

            work.SetManagers(null, tableManager, figureManager);

            TableDTO tableDTO_good = new TableDTO { Id = 1 };
            TableDTO tableDTO_bad = new TableDTO { Id = 124 };
            //Act
            TableService service = new TableService(work.Object);
            OperationDetails details_good = await service.DeteteTable(tableDTO_good);
            OperationDetails details_bad = await service.DeteteTable(tableDTO_bad);

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
            MockUnitOfWork work = new MockUnitOfWork();

            

            MockUserManager userManager = new MockUserManager(work.GetUserStore().Object)
                .MockFindByIdAsync();

            MockTableManager tableManager = new MockTableManager()
                .MockDeleteAsync()
                .MockFindByIdAsync();

            MockFigureManager figureManager = new MockFigureManager()
               .MockDeleteAsync();

            MockApplicationUser userWithNoTables = new MockApplicationUser{ Id = 20 }
            .SetupApplicationUser();

            work.SetManagers(userManager, tableManager, figureManager);
            work.AddUserToTable(userWithNoTables);

            UserDTO user_good = new UserDTO { Id = 1 };
            UserDTO user_bad = new UserDTO { Id = 123};
            UserDTO user_with_no_table = new UserDTO { Id = userWithNoTables.Id};


            //Act
            TableService service = new TableService(work.Object);
            OperationDetails details_good = await service.DeteteUserTables(user_good);
            OperationDetails details_bad = await service.DeteteUserTables(user_bad);
            OperationDetails details_noTable = await service.DeteteUserTables(user_with_no_table);

            //Assert
            Assert.IsTrue(details_good.Succedeed, "Failed while deleting good user tables.");
            Assert.IsFalse(details_bad.Succedeed, "Succes while deleting bad user tables.");
            Assert.IsTrue(details_noTable.Succedeed, "Failed while deleting tables for user with no tables.");
        }
        #endregion

        #region GET_FIGURES_ON_TABLE
        [TestMethod()]
        public async Task GetFiguresOnTableTest()
        {
            //Arrange
            MockUnitOfWork work = new MockUnitOfWork();

            MockFigureManager figureManager = new MockFigureManager()
                .MockGetFiguresForTable();

            work.SetManagers(null, null, figureManager);

            TableDTO table_good = new TableDTO { Id = 1 };
            TableDTO table_bad = new TableDTO { Id = 123 };
            
            //Act
            TableService service = new TableService(work.Object);
            var result_good = await service.GetFiguresOnTable(table_good);
            var result_bad = await service.GetFiguresOnTable(table_bad);

            //Assert
            Assert.AreEqual(result_good.Count(), 40, "Bad number of figures on the fresh table");
            Assert.IsNull(result_bad, "Not 0 figures on table with bad Id");
        }
        #endregion

        #region GET_TABLE
        [TestMethod()]
        public async Task GetTableTest()
        {
            //Arrange
            MockUnitOfWork work = new MockUnitOfWork();

            MockTableManager tableManager = new MockTableManager()
                .MockFindByIdAsync();

            work.SetManagers(null, tableManager);

            TableDTO table_good = new TableDTO{ Id = 1 };
            TableDTO table_bad = new TableDTO{ Id = 123 };
  
            //Act
            TableService service = new TableService(work.Object);

            TableDTO result_good = await service.GetTable(table_good);
            TableDTO result_bad = await service.GetTable(table_bad);

            //Assert
            Assert.AreEqual(result_good.Id, table_good.Id, "Not the same id returned for good table.");
            Assert.IsNotNull(result_good.Opponents, "No opponents for good table");
            Assert.IsNull(result_bad, "Not null table with bad table Id");
        }
        #endregion

        #region GET_ALL_TABLES
        [TestMethod()]
        public async Task GetAllTablesTest()
        {
            //Arrange
            MockUnitOfWork work = new MockUnitOfWork();

            MockTableManager tableManager = new MockTableManager()
                .MockGetAllTables();

            int tablesNumber = work._userNum * work._tablePerUserNum;
            work.SetManagers(null, tableManager);

            //Act
            TableService service = new TableService(work.Object);
            var result = await service.GetAllTables();

            //Assert
            Assert.AreEqual(result.Count(), tablesNumber);
        }
        #endregion

        #region GET_USER_TABLES
        [TestMethod()]
        public async Task GetUserTablesTest()
        {
            //Arrange
            MockUnitOfWork work = new MockUnitOfWork();

            MockUserManager userManager = new MockUserManager(work.GetUserStore().Object)
                .MockFindByNameAsync();

            MockTableManager tableManager = new MockTableManager()
                .MockGetUserTables();

            int tablesNumber = work._tablePerUserNum;
            work.SetManagers(userManager, tableManager);

            UserDTO user_good = new UserDTO { UserName = "username_1" };
            UserDTO user_bad = new UserDTO { UserName = "bad_username" };

            //Act
            TableService service = new TableService(work.Object);
            var result_good = await service.GetUserTables(user_good);
            var result_bad = await service.GetUserTables(user_bad);

            //Assert
            Assert.AreEqual(result_good.Count(), tablesNumber);
            Assert.IsNull(result_bad);
        }
        #endregion

        #region GET_AVAILABLE_TABLES
        [TestMethod()]
        public async Task GetAvailableTablesTest()
        {
            //Arrange
            MockUnitOfWork work = new MockUnitOfWork();

            MockUserManager userManager = new MockUserManager(work.GetUserStore().Object)
                .MockFindByNameAsync();

            MockTableManager tableManager = new MockTableManager()
                .MockGetAvailableTables();

            MockApplicationUser test_user = new MockApplicationUser
            {
                Id = 20,
                UserName = "test_username",
                Tables = new List<MockTable>
                {
                    new MockTable
                    {
                        Id = 20,
                        Opponents = new List<MockApplicationUser>
                        {
                            new MockApplicationUser{ Id = 100, UserName = "username_100", Email = "email_100@gmail.com",
                                PlayerProfile = new MockPlayerProfile{ Name = "name_100", Surname = "surname_100"} }.SetupApplicationUser(),

                            new MockApplicationUser { Id = 101, UserName = "username_101", Email = "email_101@gmail.com",
                                PlayerProfile = new MockPlayerProfile{ Name = "name_101", Surname = "surname_101"}}.SetupApplicationUser()
                        }
                    },
                    new MockTable
                    {
                        Id = 21,
                        Opponents = new List<MockApplicationUser>
                        {
                            new MockApplicationUser { Id = 102, UserName = "username_102", Email = "email_102@gmail.com",
                                PlayerProfile = new MockPlayerProfile{ Name = "name_102", Surname = "surname_102"}}.SetupApplicationUser()
                        }
                    }
                }
            }.SetupApplicationUser();

            work.SetManagers(userManager, tableManager);
            work.AddUserToTable(test_user);

            int goodTablesNumber = work._tablePerUserNum * (work._userNum - 1) + 1;

            UserDTO user_good = new UserDTO { UserName = "username_1" };
            UserDTO user_bad = new UserDTO { UserName = "bad_username" };

            //Act
            TableService service = new TableService(work.Object);
            var result_good = await service.GetAvailableTables(user_good);
            var result_bad = await service.GetAvailableTables(user_bad);

            //Assert
            Assert.AreEqual(result_good.Count(), goodTablesNumber);
            Assert.IsNull(result_bad);
        }
        #endregion
    }
}