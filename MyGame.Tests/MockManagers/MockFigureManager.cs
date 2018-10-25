using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.Tests.MockHelpers;
using MyGame.Tests.Models;

namespace MyGame.Tests.MockManagers
{
    internal class MockFigureManager : Mock<IFigureManager>
    {
        public MockFigureManager MockCreateAsync()
        {
            Setup(m => m.CreateAsync(
                It.IsAny<int>()
                )).ReturnsAsync(false);

            Setup(m => m.CreateAsync(
                It.Is<int>(id => id == 2)))
                .ReturnsAsync(true);
            return this;
        }

        public MockFigureManager MockDeleteAsync()
        {
            Setup(m => m.DeleteAsync(
                It.IsAny<int>()))
                .ReturnsAsync(false);

            Setup(m => m.DeleteAsync(
                It.Is<int>(id => id == ServiceDataToUse.Table.Id)))
                .ReturnsAsync(true);
            return this;
        }

        public MockFigureManager MockGetFiguresForTable()
        {
            Setup(m => m.GetFiguresForTable(
                It.IsAny<int>()))
                .Returns(GetDbSetFigures(null));

            Setup(m => m.GetFiguresForTable(
                It.Is<int>(id => id == ServiceDataToUse.Game.Id)))
                .Returns(GetDbSetFigures( new List<Figure> { ServiceDataToUse.Figure }));
            return this;
        }

        private IQueryable<Figure> GetDbSetFigures(List<Figure> figures)
        {
            return MockDbSet.GetDataSet(figures).Object;
        }
    }
}
