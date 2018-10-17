using Moq;
using MyGame.DAL.Entities;
using MyGame.Tests.MockEnity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MyGame.Tests.MockHelpers
{
    
    internal class MockDbSetFigure : Mock<DbSet<Figure>>
    {
        public IQueryable<Figure> Figures { get; set; }
        public MockDbSetFigure(IEnumerable<Figure> figures)
        {
            if(figures == null)
                Figures = new List<Figure>().AsQueryable();

            else
            Figures = figures.AsQueryable();
        }

        public MockDbSetFigure SetupDbSetFigure()
        {
            As<IDbAsyncEnumerable<Figure>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Figure>(Figures.GetEnumerator()));

            As<IQueryable<Figure>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Figure>(Figures.Provider));

            As<IQueryable<Figure>>()
                .Setup(m => m.Expression)
                .Returns(Figures.Expression);

            As<IQueryable<Figure>>()
                .Setup(m => m.ElementType)
                .Returns(Figures.ElementType);

            As<IQueryable<Figure>>()
                .Setup(m => m.GetEnumerator())
                .Returns(Figures.GetEnumerator());
            return this;
        }

    }

    internal class MockDbSetTable : Mock<DbSet<Table>>
    {
        public IQueryable<Table> Tables { get; set; }
        public MockDbSetTable(IEnumerable<Table> tables)
        {
            if (tables == null)
                Tables = new List<Table>().AsQueryable();

            else
                Tables = tables.AsQueryable();
        }

        public MockDbSetTable SetupDbSetTable()
        {
            As<IDbAsyncEnumerable<Table>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Table>(Tables.GetEnumerator()));

            As<IQueryable<Table>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Table>(Tables.Provider));

            As<IQueryable<Table>>()
                .Setup(m => m.Expression)
                .Returns(Tables.Expression);

            As<IQueryable<Table>>()
                .Setup(m => m.ElementType)
                .Returns(Tables.ElementType);

            As<IQueryable<Table>>()
                .Setup(m => m.GetEnumerator())
                .Returns(Tables.GetEnumerator());
            return this;
        }

    }

}