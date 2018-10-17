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
                .MockFindById();

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
                .MockFindById();

            MockFigureManager figureManager = new MockFigureManager()
               .MockDeleteAsync();

            MockApplicationUser userWithNoTables = new MockApplicationUser
            { Id = 20 }.SetupApplicationUser();

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

        [TestMethod()]
        public void GetFiguresOnTableTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetTableTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTablesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUserTablesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAvailableTablesTest()
        {
            Assert.Fail();
        }
    }
}