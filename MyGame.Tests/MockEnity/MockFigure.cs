using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.DAL.Entities;

namespace MyGame.Tests.MockEnity
{
    internal class MockFigure : Mock<Figure>
    {
        internal int Id { get; set; }
        internal Colors Color { get; set; }
        internal int X { get; set; }
        internal int Y { get; set; }
        internal  MockTable Table { get; set; }

        internal MockFigure SetupFigure()
        {
            Setup(m => m.Id).Returns(Id);
            Setup(m => m.Color).Returns(Color);
            Setup(m => m.X).Returns(X);
            Setup(m => m.Y).Returns(Y);
            Setup(m => m.Table).Returns(Table.Object);

            return this;
        }
    }
}
