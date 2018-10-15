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
        List<UserDTO> users = new List<UserDTO>
            {
                new UserDTO{ Id = 1, Email = "email_1@gmail.com", Password = "111111", UserName = "username_1", Name = "name_1", Surname = "Surname_1"},
                new UserDTO{ Id = 2, Email = "email_2@gmail.com", Password = "222222", UserName = "username_2", Name = "name_2", Surname = "Surname_2"},
                new UserDTO{ Id = 3, Email = "email_3@gmail.com", Password = "333333", UserName = "username_3", Name = "name_3", Surname = "Surname_3"},
                new UserDTO{ Id = 4, Email = "email_4@gmail.com", Password = "444444", UserName = "username_4", Name = "name_4", Surname = "Surname_4"}
            };


        internal MockUserService MockAuthenticate()
        {
            Setup(m => m.Authenticate(
                It.Is<UserDTO>(u => users.FirstOrDefault(f => (f.Email == u.Email) && (f.Password == u.Password)) != null)
                )).ReturnsAsync(new ClaimsIdentity());
            return this;
        }

        internal MockUserService MockCreate()
        {
            List<UserDTO> testList = new List<UserDTO>(users);
            Setup(m => m.Create(
                It.Is<UserDTO>(u => (testList.FirstOrDefault(f => (f.Email == u.Email) || (f.UserName == u.UserName)) == null) &&
                                    (testList.Append(u).Last().Equals(u)))
                )).ReturnsAsync(new OperationDetails(true));

            Setup(m => m.Create(
                It.Is<UserDTO>(u => (users.FirstOrDefault(f => (f.Email == u.Email) || (f.UserName == u.UserName))) != null)
                )).ReturnsAsync(new OperationDetails(false));
            return this;
        }

        internal MockUserService MockDelete()
        {
            List<UserDTO> testList = new List<UserDTO>(users);
            Setup(m => m.Delete(
                It.Is<UserDTO>(u => ((testList.FirstOrDefault(f => (f.Id == u.Id)) != null) &&
                                    (testList.Remove(testList.Find(f => f.Id == u.Id)))))
                )).ReturnsAsync(new OperationDetails(true));

            Setup(m => m.Delete(
                It.Is<UserDTO>(u => testList.FirstOrDefault(f => (f.Id == u.Id)) == null )
                )).ReturnsAsync(new OperationDetails(false));

            return this;
        }

        internal MockUserService GetAllUsers()
        {
            Setup(m => m.GetAllUsers()).ReturnsAsync(users);
            return this;
        }
    }
}
