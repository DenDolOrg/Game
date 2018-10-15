using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyGame.BLL.DTO;
using MyGame.Controllers;
using MyGame.Infrastructure;
using MyGame.Models;
using MyGame.Tests.MockHelpers;
using MyGame.Tests.Models;
using MyGame.Tests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MyGame.Controllers.Tests
{
    [TestClass()]
    public class TableControllerTests
    {
        #region DATA_TO_USE
        //List<UserDTO> users = new List<UserDTO>
        //    {
        //        new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
        //        new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
        //        new UserDTO{ Id = 3, Email = "email_3@gmail.com", Password = "333333", UserName = "username_3", Name = "name_3", Surname = "Surname_3"},
        //        new UserDTO{ Id = 4, Email = "email_4@gmail.com", Password = "444444", UserName = "username_4", Name = "name_4", Surname = "Surname_4"}
        //    };
        #endregion

        #region TABLE_LIST
        [TestMethod()]
        public void TableListTest()
        {
            //Arrange
            string all = "all";
            string available = "available";
            string myTables = "myTables";

            string allResult = "/Table/GetAllTables";
            string availableResult = "/Table/GetAvailableTables";
            string myTablesResult = "/Table/GetUserTables";
            //Act
            TableController tableController = new TableController(null);

            TableActionModel result_all = (TableActionModel)tableController.TableList(all).Model;
            TableActionModel result_available = (TableActionModel)tableController.TableList(available).Model;
            TableActionModel result_myTables = (TableActionModel)tableController.TableList(myTables).Model;

            //Assert
            Assert.AreEqual(allResult, result_all.ActionName, "Do not return good action name for All");
            Assert.AreEqual(availableResult, result_available.ActionName, "Do not return good action name for Available");
            Assert.AreEqual(myTablesResult, result_myTables.ActionName, "Do not return good action name for My tables");
        }
        #endregion

        #region USER_TABLES
        [TestMethod()]
        public async Task GetUserTablesTest()
        {
            //Arrange
            UserDTO good_user = new UserDTO { UserName = "username_2" };
            UserDTO bad_user = new UserDTO { UserName = "bad_username" };

            UserTestModel userWithoutTables = new UserTestModel
            {
                UserDTO = new UserDTO { UserName = "testUserName" },
                Tables = new List<TableDTO>()
            };

            var mockTableService = new MockTableService()
                .MockGetUserTables(userWithoutTables);

            List<TableDTO> good_tables = mockTableService.Tables.GetRange(1 * 3, 3);

            //Act
            TableController tableController = new TableController(null, mockTableService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(good_user.UserName).CustomHttpContextBase);
            var goodTablesRes = await tableController.GetUserTables();

            HttpContextManager.SetCurrentContext(new MockHttpContext(bad_user.UserName).CustomHttpContextBase);
            var badTablesRes = await tableController.GetUserTables();

            HttpContextManager.SetCurrentContext(new MockHttpContext(userWithoutTables.UserDTO.UserName).CustomHttpContextBase);
            var noTablesRes = await tableController.GetUserTables();

            //Assert
            Assert.AreEqual(Json.Encode(good_tables), Json.Encode(goodTablesRes.Data), "Are not equal good tables");
            Assert.AreEqual(Json.Encode(noTablesRes.Data), Json.Encode(new List<TableDTO>()), "Not empty list of tables for user with no tables");
            Assert.IsNull(badTablesRes, "Not null tables for user with bad bad username");
        }
        #endregion

        #region ALL_TABLES
        [TestMethod()]
        public async Task GetAllTablesTest()
        {
            //Arrange
            var mockTableService = new MockTableService()
                .MockGetAllTables();

            List<TableDTO> tables = mockTableService.Tables;
            //Act
            TableController tableController = new TableController(null,mockTableService.Object);
            var result = await tableController.GetAllTables();

            //Assert
            Assert.AreEqual(Json.Encode(tables), Json.Encode(result.Data));
        }
        #endregion

        #region AVAILABLE_TABLES
        [TestMethod()]
        public async Task GetAvailableTablesTest()
        {
            //Arrange

            var mockTableService = new MockTableService()
                .MockGetAvailableTables();

            mockTableService.UserModels.ElementAt(0).Tables.ElementAt(1).Opponents.Add(mockTableService.UserModels.ElementAt(1).UserDTO);
            mockTableService.UserModels.ElementAt(1).Tables.Add(mockTableService.UserModels.ElementAt(0).Tables.ElementAt(1));

            mockTableService.UserModels.ElementAt(3).Tables.ElementAt(0).Opponents.Add(mockTableService.UserModels.ElementAt(1).UserDTO);
            mockTableService.UserModels.ElementAt(1).Tables.Add(mockTableService.UserModels.ElementAt(3).Tables.ElementAt(0));

            UserDTO good_user1 = new UserDTO { UserName = "username_2" };
            UserDTO good_user2 = new UserDTO { UserName = "username_1" };
            UserDTO bad_user = new UserDTO { UserName = "bad_username" };

            //Act
            TableController tableController = new TableController(null, mockTableService.Object);
            HttpContextManager.SetCurrentContext(new MockHttpContext(good_user1.UserName).CustomHttpContextBase);

            var result1 = await tableController.GetAvailableTables();

            HttpContextManager.SetCurrentContext(new MockHttpContext(good_user2.UserName).CustomHttpContextBase);

            var result2 = await tableController.GetAvailableTables();

            HttpContextManager.SetCurrentContext(new MockHttpContext(bad_user.UserName).CustomHttpContextBase);

            var result_bad = await tableController.GetAvailableTables();

            List<TableDTO> resList_1 = Json.Decode<List<TableDTO>>(Json.Encode(result1.Data));
            List<TableDTO> resList_2 = Json.Decode<List<TableDTO>>(Json.Encode(result2.Data));

            //Assert
            Assert.AreEqual(resList_1.Count, 15 - 3 - 2, "Bad number of tables for 2 user.");
            Assert.AreEqual(resList_2.Count, 15 - 3 - 1, "Bad number of tables for 1 user.");
            Assert.IsNull(result_bad, "List of tables for user with bad username is ot empty");

        }
        #endregion

        #region CREATE_TABLE
        [TestMethod()]
        public async Task CreateNewtableTest()
        {
            //Arrange 
            UserDTO user_good = new UserDTO { UserName = "username_3" };
            UserDTO user_bad = new UserDTO { UserName = "bad_username" };

            var mockTableService = new MockTableService()
                .MockCreateTable();
            
            //Act
            TableController tableController = new TableController(null, mockTableService.Object);

            HttpContextManager.SetCurrentContext(new MockHttpContext(user_good.UserName).CustomHttpContextBase);
            var result_good = await tableController.CreateNewtable();

            HttpContextManager.SetCurrentContext(new MockHttpContext(user_bad.UserName).CustomHttpContextBase);
            var result_bad = await tableController.CreateNewtable();

            //Assert
            Assert.IsNotNull(result_good, "Can't create table for valid username.");
            Assert.IsNull(result_bad, "Can create table for invalid username.");
            Assert.IsTrue(mockTableService.UserModels.ElementAt(2).Tables.Count == 4, "Fail while adding to table list.");
        }
        #endregion

        #region DELETE
        [TestMethod()]
        [ExpectedException(typeof(HttpException))]
        public async Task DeleteTest()
        {
            //Arrange
            TableDTO good_table = new TableDTO { Id = 1 };
            TableDTO bad_table = new TableDTO { Id = 124 };

            var mockTableService = new MockTableService()
                .MockDeleteTable();

            //Act
            TableController tableController = new TableController(null, mockTableService.Object);
            await tableController.Delete(good_table.Id);
            await tableController.Delete(bad_table.Id);

            //Assert
            Assert.IsTrue(mockTableService.Tables.First().Id == 2);
        }
        #endregion
    }
}