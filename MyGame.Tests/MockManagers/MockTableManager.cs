using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.BLL.DTO;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.Tests.MockEnity;
using MyGame.Tests.MockHelpers;

namespace MyGame.Tests.MockManagers
{
    internal class MockTableManager : Mock<ITableManager>
    {
        internal List<MockApplicationUser> Users { get; set; }
        internal List<MockTable> Tables { get; set; }

        internal MockTableManager MockCreateAsync()
        {
            Setup(m => m.CreateAsync(
                It.IsAny<Table>()
                )).ReturnsAsync(false);

            Setup(m => m.CreateAsync(
                It.Is<Table>(t => (from tl in Tables
                                   where tl.Id == t.Id
                                   select tl).Count() == 0 && CanCreateId(ref t))))
                                   .ReturnsAsync(true);
            return this;
        }
        internal MockTableManager MockDeleteAsync()
        {
            Setup(m => m.DeleteAsync(
                It.IsAny<Table>()
                )).ReturnsAsync(false);

            Setup(m => m.DeleteAsync(
                It.Is<Table>(t => (from tl in Tables
                                   where tl.Id == t.Id
                                   select tl).Count() == 1)))
                                   .ReturnsAsync(true);
            return this;
        }

        internal MockTableManager MockFindByIdAsync()
        {
            Setup(m => m.FindByIdAsync(
                It.IsAny<int>()
                )).ReturnsAsync((Table)null);

            Setup(m => m.FindByIdAsync(
                It.Is<int>(id => (from tl in Tables
                                  where tl.Id == id
                                  select tl).Count() == 1)))
                                  .ReturnsAsync((int id) => (from tl in Tables
                                                        where tl.Id == id
                                                        select tl.Object).First());
            return this;
        }

        internal MockTableManager MockGetAllTables()
        {
            Setup(m => m.GetAllTabes())
                .Returns(() => GetDbSetTables((from tl in Tables
                          select tl.Object).ToList()));
            return this;
        }

        internal MockTableManager MockGetUserTables()
        {
            Setup(m => m.GetTablesForUser(
                It.IsAny<int>()))
                .Returns(() => GetDbSetTables(null));
     

            Setup(m => m.GetTablesForUser(
                It.Is<int>(id => (from ul in Users
                                  where ul.Id == id
                                  select ul).Count() != 0)))
                                  .Returns((int id) => GetDbSetTables((from tl in Tables
                                                                       where (from o in tl.Opponents
                                                                              where o.Id == id
                                                                              select o).Count() == 1
                                                                       select tl.Object).ToList()));
            return this;
        }

        internal MockTableManager MockGetAvailableTables()
        {
            Setup(m => m.GetAvailableTables(
                It.IsAny<int>()))
                .Returns(() => GetDbSetTables(null));


            Setup(m => m.GetAvailableTables(
                It.Is<int>(id => (from ul in Users
                                  where ul.Id == id
                                  select ul).Count() == 1)))
                                  .Returns((int id) => GetDbSetTables((from tl in Tables
                                                                       where tl.Opponents.Count() == 1
                                                                       where (from o in tl.Opponents
                                                                              where o.Id == id
                                                                              select o).Count() == 0
                                                                       select tl.Object).ToList()));
            return this;
        }

        #region HELPERS

        private IQueryable<Table> GetDbSetTables(IEnumerable<Table> tables)
        {
            return new MockDbSetTable(tables)
                .SetupDbSetTable()
                .Object;
        }

        private bool CanCreateId(ref Table table)
        {
            table.Id = Tables.Count + 1;
            return true;
        }

        internal void AddData(ref List<MockApplicationUser> users, ref List<MockTable> tables)
        {
            Users = users;
            Tables = tables;
        }
        #endregion

    }
}
