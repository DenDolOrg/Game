using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using MyGame.BLL.Interfaces;
using MyGame.BLL.DTO;
using System.Security.Claims;
using MyGame.Models;
using MyGame.BLL.Infrastructure;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MyGame.Tests.Services
{
    internal class MockUserService : Mock<IUserService>
    {
        internal List<UserDTO> Users = new List<UserDTO>
            {
                new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
                new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
            };


        internal MockUserService MockAuthenticate()
        {
            Setup(m => m.Authenticate(
                It.Is<UserDTO>(u => (from ul in Users
                                     where ul.Email == u.Email && ul.Password == u.Password
                                     select ul).Count() == 1)))
                                     .ReturnsAsync(new ClaimsIdentity());
            return this;
        }

        internal MockUserService MockCreate()
        {
            Setup(m => m.Create(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.Create(
                It.Is<UserDTO>(u => (from ul in Users
                                     where ul.Email == u.Email || ul.UserName == u.UserName
                                     select ul).Count() == 0)))
                                     .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        internal MockUserService MockDelete()
        {
            Setup(m => m.Delete(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.Delete(
                It.Is<UserDTO>(u => (from ul in Users
                                     where ul.Id == u.Id
                                     select ul).Count() == 1)))
                                     .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        internal MockUserService GetAllUsers()
        {
            Setup(m => m.GetAllUsers()).ReturnsAsync(Users);
            return this;
        }

        internal MockUserService GetUser()
        {
            Setup(m => m.GetUser(
                It.IsAny<UserDTO>()
                )).ReturnsAsync((UserDTO)null);

            Setup(m => m.GetUser(
                It.Is<UserDTO>(u => (from ul in Users
                                     where ul.UserName == u.UserName
                                     select ul).Count() == 1)))
                                     .ReturnsAsync((UserDTO u) => (from ul in Users
                                                                   where ul.UserName == u.UserName
                                                                   select ul).First());
            return this;
        }

    }
}
