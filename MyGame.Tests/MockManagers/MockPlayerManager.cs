using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.Tests.Models;

namespace MyGame.Tests.MockManagers
{
    internal class MockPlayerManager : Mock<IPlayerManager>
    {
        public MockPlayerManager MockDeleteAsync()
        {
            Setup(m => m.DeleteAsync(
                It.IsAny<PlayerProfile>()))
                .ReturnsAsync(false);

            Setup(m => m.DeleteAsync(
                It.Is<PlayerProfile>(p => p.Id == ServiceDataToUse.User.PlayerProfile.Id)))
                .ReturnsAsync(true);

            return this;
        }

        public MockPlayerManager MockCreateAsync()
        {
            Setup(m => m.CreateAsync(
                It.IsAny<PlayerProfile>()))
                .ReturnsAsync(false);

            Setup(m => m.CreateAsync(
                It.Is<PlayerProfile>(p => p.Id == 2)))
                .ReturnsAsync(true);

            return this;
        }
    }
}
