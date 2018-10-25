using Moq;
using MyGame.DAL.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MyGame.Tests.MockHelpers
{
    public static class MockDbSet
    {

        public static Mock<DbSet<T>> GetDataSet<T>(List<T> list) where T : class
        {
            var dbSet = new Mock<DbSet<T>>();

            if (list == null)
                list = new List<T>();
            var listToReturn = list.AsQueryable();

            dbSet.As<IDbAsyncEnumerable<T>>()
                    .Setup(m => m.GetAsyncEnumerator())
                    .Returns(new TestDbAsyncEnumerator<T>(listToReturn.GetEnumerator()));

            dbSet.As<IQueryable<T>>()
                    .Setup(m => m.Provider)
                    .Returns(new TestDbAsyncQueryProvider<T>(listToReturn.Provider));

            dbSet.As<IQueryable<T>>()
                    .Setup(m => m.Expression)
                    .Returns(listToReturn.Expression);

            dbSet.As<IQueryable<T>>()
                    .Setup(m => m.ElementType)
                    .Returns(listToReturn.ElementType);

            dbSet.As<IQueryable<T>>()
                    .Setup(m => m.GetEnumerator())
                    .Returns(listToReturn.GetEnumerator());

            return dbSet;

        }

        

    }
}