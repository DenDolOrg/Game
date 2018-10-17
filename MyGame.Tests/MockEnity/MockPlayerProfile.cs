using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.DAL.Entities;

namespace MyGame.Tests.MockEnity
{
    internal class MockPlayerProfile : Mock<PlayerProfile>
    {
        internal int Id { get; set; }

        internal new string Name { get; set; }

        internal string Surname { get; set; }

        internal MockApplicationUser ApplicationUser { get; set; }

        internal MockPlayerProfile SetupPlayerProfile()
        {
            Setup(m => m.ApplicationUser).Returns(ApplicationUser.Object);
            Setup(m => m.Id).Returns(Id);
            Setup(m => m.Name).Returns(Name);
            Setup(m => m.Surname).Returns(Surname);

            return this;
        }
    }
}
