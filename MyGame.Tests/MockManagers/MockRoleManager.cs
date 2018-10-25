using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Identity;

namespace MyGame.Tests.MockManagers
{
    internal class MockRoleManager : Mock<ApplicationRoleManager>
    {      

        public MockRoleManager(RoleStore<ApplicationRole, int, UserRole> store) : base(store){ }
        public MockRoleManager MockFindByNameAsync()
        {
            Setup(m => m.FindByNameAsync(
                It.IsAny<string>()))
                .ReturnsAsync((ApplicationRole)null);

            Setup(m => m.FindByNameAsync(
                It.Is<string>(s => s == "user")))
                .ReturnsAsync(new ApplicationRole { Id = 1, Name = "user"});
            return this;
        }

        public MockRoleManager MockCreateAsync()
        {
            Setup(m => m.CreateAsync(
                It.IsAny<ApplicationRole>()))
                .ReturnsAsync(IdentityResult.Success);
            return this;
        }

        public MockRoleManager MockRoles(List<ApplicationRole> roles)
        {
            Setup(m => m.Roles).Returns(roles.AsQueryable());
            return this;
        }
    }
}
