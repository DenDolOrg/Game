using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Moq;
using MyGame.DAL.Entities;

namespace MyGame.Tests.MockStores
{
    internal class MockRoleStore : Mock<IRoleStore<ApplicationRole, int>>
    {
    }
}
