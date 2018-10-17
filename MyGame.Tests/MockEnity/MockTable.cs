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
        public int Id { get; set; }

        public List<MockApplicationUser> Opponents { get; set; }

        public DateTime CreationTime { get; set; }
        internal MockTable()
        {
            Opponents = new List<MockApplicationUser>();
        }
        public MockTable SetupTable()
        {
            Setup(m => m.Id).Returns(Id);
            Setup(m => m.CreationTime).Returns(CreationTime);
            Setup(m => m.Opponents).Returns(() => (from o in Opponents
                                                   select o.Object).ToList());
            return this;
        }
    }
}
