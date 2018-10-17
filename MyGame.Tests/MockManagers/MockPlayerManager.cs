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
    internal class MockPlayerManager : Mock<IPlayerManager>
    {
        List<MockApplicationUser> Users { get; set; }


        internal void AddData(ref List<MockApplicationUser> users)
        {
            Users = users;
        }
    }
}
