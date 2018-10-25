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
using MyGame.Tests.Models;

namespace MyGame.Tests.Services
{
    internal class MockGameService : Mock<IGameService>
    {
        internal MockGameService MockDeleteUserGames()
        {
            Setup(m => m.DeteteUserGame(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.DeteteUserGame(
                It.Is<UserDTO>(u => u.Id == ControllerDataToUse.UserDTO.Id)))
                .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        internal MockGameService MockGetUserGames()
        {
            Setup(m => m.GetUserGames(
                It.IsAny<UserDTO>()))
                .ReturnsAsync((ICollection<GameDTO>)null);

            Setup(m => m.GetUserGames(
                It.Is<UserDTO>(u => u.UserName == ControllerDataToUse.UserDTO.UserName)))
                .ReturnsAsync(new List<GameDTO> { ControllerDataToUse.GameDTO });
            return this;
        }

        internal MockGameService MockGetAllGames()
        {
            Setup(m => m.GetAllGames())
            .ReturnsAsync(new List<GameDTO> { ControllerDataToUse.GameDTO });
            return this;
        }

        internal MockGameService MockGetAvailableGames()
        {
            Setup(m => m.GetAvailableGames(
                It.IsAny<UserDTO>()))
                .ReturnsAsync((List<GameDTO>)null);

            Setup(m => m.GetAvailableGames(
                It.Is<UserDTO>(u => u.UserName == ControllerDataToUse.UserDTO.UserName)))
                .ReturnsAsync(new List<GameDTO> { ControllerDataToUse.GameDTO });
            return this;
        }

        internal MockGameService MockCreateGame()
        {
            Setup(m => m.CreateNewGame(
                It.IsAny<UserDTO>()))
                .ReturnsAsync(new OperationDetails(false));

            Setup(m => m.CreateNewGame(
                It.Is<UserDTO>(u => u.UserName == ControllerDataToUse.UserDTO.UserName)))
                .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        internal MockGameService MockDeleteGame()
        {
            Setup(m => m.DeteteGame(
                It.IsAny<GameDTO>()))
                .ReturnsAsync(new OperationDetails(false));

            Setup(m => m.DeteteGame(
                It.Is<GameDTO>(t => t.Id == ControllerDataToUse.GameDTO.Id)))
                .ReturnsAsync(new OperationDetails(true));
            return this;
        }       
    }
}
