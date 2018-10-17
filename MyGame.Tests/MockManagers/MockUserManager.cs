using Microsoft.AspNet.Identity;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Identity;
using MyGame.Tests.MockEnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.MockManagers
{
    internal class MockUserManager : Mock<ApplicationUserManager>
    {
        List<MockApplicationUser> Users;
        internal MockUserManager(IUserStore<ApplicationUser, int> store) : base(store)
        {
        }

        internal MockUserManager MockFindByNameAsync()
        {
            Setup(m => m.FindByNameAsync(
                It.IsAny<string>()
                )).ReturnsAsync((ApplicationUser)null);

            Setup(m => m.FindByNameAsync(
                It.Is<string>(s => (from ul in Users
                                    where ul.UserName == s
                                    select ul).Count() == 1)))
                                    .ReturnsAsync((string s) => (from ul in Users
                                                                 where ul.UserName == s
                                                                 select ul.Object).First());
            return this;
        }

        internal MockUserManager MockFindByIdAsync()
        {
            Setup(m => m.FindByIdAsync(
             It.IsAny<int>()
             )).ReturnsAsync((ApplicationUser)null);

            Setup(m => m.FindByIdAsync(
                It.Is<int>(id => (from ul in Users
                                    where ul.Id == id
                                    select ul).Count() == 1)))
                                    .ReturnsAsync((int id) => (from ul in Users
                                                                 where ul.Id == id
                                                                 select ul.Object).First());
            return this;
        }

        internal void AddData(ref List<MockApplicationUser> users)
        {
            Users = users;
        }
    }
}
