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
using MyGame.Tests.Models;

namespace MyGame.Tests.Services
{
    internal class MockUserService : Mock<IUserService>
    {
        public MockUserService MockAuthenticate()
        {
            Setup(m => m.Authenticate(
                It.Is<UserDTO>(u => u.Email == ControllerDataToUse.UserDTO.Email && u.Password == ControllerDataToUse.UserDTO.Password )))
                .ReturnsAsync(new ClaimsIdentity());
            return this;
        }

        public MockUserService MockCreate()
        {
            Setup(m => m.Create(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.Create(
                It.Is<UserDTO>(u => u.UserName != ControllerDataToUse.UserDTO.UserName && u.Email != ControllerDataToUse.UserDTO.Email)))
                .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        public MockUserService MockDelete()
        {
            Setup(m => m.Delete(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.Delete(
                It.Is<UserDTO>(u => u.Id == ControllerDataToUse.UserDTO.Id)))
                .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        public MockUserService MockGetAllUsers()
        {
            Setup(m => m.GetAllUsers()).ReturnsAsync(new List<UserDTO> { ControllerDataToUse.UserDTO });
            return this;
        }

        public MockUserService MockGetUser()
        {
            Setup(m => m.GetUser(
                It.IsAny<UserDTO>()
                )).ReturnsAsync((UserDTO)null);

            Setup(m => m.GetUser(
                It.Is<UserDTO>(u => u.UserName == ControllerDataToUse.UserDTO.UserName)))
                .ReturnsAsync(ControllerDataToUse.UserDTO);
            return this;
        }

    }
}
