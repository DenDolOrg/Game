using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using MyGame.DAL.Entities;
using MyGame.Tests.MockEnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.MockManagers
{
    internal class MockRoleStore : Mock<RoleStore<ApplicationRole, int, UserRole>>
    {

    }


}
