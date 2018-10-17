using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.Tests.MockEnity;

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

        internal MockTableManager MockFindById()
        {
            Setup(m => m.FindById(
                It.IsAny<int>()
                )).Returns((Table)null);

            Setup(m => m.FindById(
                It.Is<int>(id => (from tl in Tables
                                  where tl.Id == id
                                  select tl).Count() == 1)))
                                  .Returns((int id) => (from tl in Tables
                                                        where tl.Id == id
                                                        select tl.Object).First());
            return this;
        }



        #region HELPERS


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
