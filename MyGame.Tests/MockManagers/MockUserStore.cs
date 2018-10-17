using Microsoft.AspNet.Identity;
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
    internal class MockUserStore : Mock<IUserStore<ApplicationUser, int>>
    {
        //List<MockApplicationUser> Users;

        //internal MockUserStore MockFindByIdAsync()
        //{
        //    Setup(m => m.FindByIdAsync(
        //     It.IsAny<int>()
        //     )).ReturnsAsync((ApplicationUser)null);

        //    Setup(m => m.FindByIdAsync(
        //        It.Is<int>(id => (from ul in Users
        //                          where ul.Id == id
        //                          select ul).Count() == 1)))
        //                            .ReturnsAsync((int id) => (from ul in Users
        //                                                       where ul.Id == id
        //                                                       select ul.Object).First());
        //    return this;
        //}

        //internal MockUserStore AddData(ref List<MockApplicationUser> users)
        //{
        //    Users = users;
        //    return this;
        //}
    }
}
