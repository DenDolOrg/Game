using Microsoft.AspNet.Identity;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Identity;
using MyGame.Tests.MockHelpers;
using MyGame.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.MockManagers
{
    internal class MockUserManager : Mock<ApplicationUserManager>
    {
        public MockUserManager(IUserStore<ApplicationUser, int> store) : base(store)
        {
        }

        public MockUserManager MockFindByNameAsync()
        {
            Setup(m => m.FindByNameAsync(
                It.IsAny<string>()
                )).ReturnsAsync((ApplicationUser)null);

            Setup(m => m.FindByNameAsync(
                It.Is<string>(s => s == ServiceDataToUse.User.UserName)))
                .ReturnsAsync(ServiceDataToUse.User);
            return this;
        }

        public MockUserManager MockFindByIdAsync()
        {
            Setup(m => m.FindByIdAsync(
             It.IsAny<int>()
             )).ReturnsAsync((ApplicationUser)null);

            Setup(m => m.FindByIdAsync(
                It.Is<int>(id => id == ServiceDataToUse.User.Id)))
                .ReturnsAsync(ServiceDataToUse.User);
            return this;
        }

        public MockUserManager MockFindByEmailAsync()
        {
            Setup(m => m.FindByEmailAsync(
             It.IsAny<string>()
             )).ReturnsAsync((ApplicationUser)null);

            Setup(m => m.FindByEmailAsync(
                It.Is<string>(s => s == ServiceDataToUse.User.Email)))
                .ReturnsAsync(ServiceDataToUse.User);
            return this;
        }

        public MockUserManager MockCheckPasswordAsync()
        {
            Setup(m => m.CheckPasswordAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser u, string p) => u.PasswordHash == p);
            return this;
        }

        public MockUserManager MockCreateIdentityAsync()
        {
            Setup(m => m.CreateIdentityAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(new ClaimsIdentity());
            IdentityResult i = new IdentityResult();
            return this;
        }

        public MockUserManager MockRemoveLoginAsync()
        {
            Setup(m => m.RemoveLoginAsync(
                 It.IsAny<int>(),
                It.IsAny<UserLoginInfo>()))
                .ReturnsAsync(IdentityResult.Failed());

            Setup(m => m.RemoveLoginAsync(
                It.Is<int>(id => id == ServiceDataToUse.User.Id),
                It.IsAny<UserLoginInfo>()))
                .ReturnsAsync(IdentityResult.Success);
            return this;
        }

        public MockUserManager MockGetRolesAsync()
        {
            Setup(m => m.GetRolesAsync(
                It.Is<int>(id => id == ServiceDataToUse.User.Id)))
                .ReturnsAsync(new List<string> { "user", "admin" });
            return this;
        }

        public MockUserManager MockRemoveFromRoleAsync()
        {
            Setup(m => m.RemoveFromRoleAsync(
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            Setup(m => m.RemoveFromRoleAsync(
                It.Is<int>(id => id == ServiceDataToUse.User.Id),
                It.Is<string>(role => role == "user" || role == "admin")))
                .ReturnsAsync(IdentityResult.Success);
            return this;
        }

        public MockUserManager MockDeleteAsync()
        {
            Setup(m => m.DeleteAsync(
                It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Failed());

            Setup(m => m.DeleteAsync(
                It.Is<ApplicationUser>(u => u.Id == ServiceDataToUse.User.Id)))
                .ReturnsAsync(IdentityResult.Success);
            return this;
        }

        public MockUserManager MockCreateAsync()
        {
            Setup(m => m.CreateAsync(
                It.IsAny<ApplicationUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success).Callback((ApplicationUser u, string s) => u.Id = 2);
            return this;
        }

        public MockUserManager MockAddToRoleAsync()
        {
            Setup(m => m.AddToRoleAsync(
                It.IsAny<int>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            Setup(m => m.AddToRoleAsync(
                It.Is<int>(id => id == 2),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            return this;
        }

        public MockUserManager MockUsers()
        {
            Setup(m => m.Users)
                .Returns(() => GetDbSetUsers(new List<ApplicationUser> { ServiceDataToUse.User }));
            return this;
        }

        private IQueryable<ApplicationUser> GetDbSetUsers(List<ApplicationUser> users)
        {
            return MockDbSet.GetDataSet(users).Object;
        }

    }
}
