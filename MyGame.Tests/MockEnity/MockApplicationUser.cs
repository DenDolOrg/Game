using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.DAL.Entities;

namespace MyGame.Tests.MockEnity
{
    internal class MockApplicationUser : Mock<ApplicationUser>
    {
        internal int Id { get; set; }
        internal string Email { get; set; }
        internal string UserName { get; set; }
        internal MockPlayerProfile PlayerProfile { get; set; }
        internal List<MockTable> Tables { get; set; }

        internal MockApplicationUser SetupApplicationUser()
        {
            if (Id != 0)
                Setup(m => m.Id).Returns(Id);

            if(Email != null)
                Setup(m => m.Email).Returns(Email);

            if (UserName != null)
                Setup(m => m.UserName).Returns(UserName);

            if (PlayerProfile != null)
                Setup(m => m.PlayerProfile).Returns(PlayerProfile.Object);

            if (Tables != null)
                Setup(m => m.Tables).Returns(() => (from t in Tables
                                                select t.Object).ToList());
            return this;
        }


    }
}
