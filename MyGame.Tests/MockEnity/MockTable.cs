using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using MyGame.DAL.Entities;
namespace MyGame.Tests.MockEnity
{
    internal class MockTable : Mock<Table>
    {
        internal int Id { get; set; }

        internal List<MockApplicationUser> Opponents { get; set; }


        internal MockTable()
        {
            Opponents = new List<MockApplicationUser>();
        }
        internal MockTable SetupTable()
        {
            Setup(m => m.Id).Returns(Id);
            Setup(m => m.Opponents).Returns(() => (from o in Opponents
                                                   select o.Object).ToList());
            return this;
        }
    }
}
