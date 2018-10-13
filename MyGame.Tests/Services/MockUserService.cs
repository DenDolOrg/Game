using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using System.Threading.Tasks;
using MyGame.BLL.Interfaces;
using MyGame.BLL.DTO;

namespace MyGame.Tests.Services
{
    public class MockUserService : Mock<IUserService>
    {
        //public MockUserService MockAuthenticate(string email, string pasword)
        //{
        //    Setup(m => m.Authenticate(
        //        It.Is<UserDTO>(u => (u.Email == email) && (u.Password == pasword))
        //        )).ReturnsAsync()
        //}
    }
}
