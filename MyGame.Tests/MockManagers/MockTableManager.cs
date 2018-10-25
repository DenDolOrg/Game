using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.MockManagers
{
    public class MockTableManager: Mock<ITableManager>
    {
        public MockTableManager MockCreateAsync()
        {

            Setup(m => m.CreateAsync(
                It.IsAny<Game>()))
                .ReturnsAsync(true).Callback((Game g) => g.Id = 2);
            return this;
        }

        public MockTableManager MockDeleteAsync()
        {
            Setup(m => m.DeleteAsync(
                It.IsAny<Game>()
                )).ReturnsAsync(false);

            Setup(m => m.DeleteAsync(
                It.Is<Game>(t => t.Id == ServiceDataToUse.Table.Id)))
                .ReturnsAsync(true);
            return this;
        }
    }
}
