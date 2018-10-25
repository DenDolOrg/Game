using Microsoft.AspNet.Identity;
using Moq;
using MyGame.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.MockStores
{
    internal class MockUserStore : Mock<IUserStore<ApplicationUser, int>>
    {
    }
}
