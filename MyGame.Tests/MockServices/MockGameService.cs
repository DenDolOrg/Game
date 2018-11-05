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
        public MockGameService MockDeleteUserGames()
        {
            Setup(m => m.DeteteUserGame(
                It.IsAny<UserDTO>()
                )).ReturnsAsync(new OperationDetails(false));

            Setup(m => m.DeteteUserGame(
                It.Is<UserDTO>(u => u.Id == ControllerDataToUse.UserDTO.Id)))
                .ReturnsAsync(new OperationDetails(true));
            return this;
        }

        public MockGameService MockGetUserGames()
        {
            Setup(m => m.GetUserGames(
                It.IsAny<UserDTO>()))
                .ReturnsAsync((ICollection<GameDTO>)null);

            Setup(m => m.GetUserGames(
                It.Is<UserDTO>(u => u.UserName == ControllerDataToUse.UserDTO.UserName)))
                .ReturnsAsync(new List<GameDTO> { ControllerDataToUse.GameDTO });
            return this;
        }

        public MockGameService MockGetAllGames()
        {
            Setup(m => m.GetAllGames())
            .ReturnsAsync(new List<GameDTO> { ControllerDataToUse.GameDTO });
            return this;
        }

        public MockGameService MockGetAvailableGames()
        {
            Setup(m => m.GetAvailableGames(
                It.IsAny<UserDTO>()))
                .ReturnsAsync((List<GameDTO>)null);

            Setup(m => m.GetAvailableGames(
                It.Is<UserDTO>(u => u.UserName == ControllerDataToUse.UserDTO.UserName)))
                .ReturnsAsync(new List<GameDTO> { ControllerDataToUse.GameDTO });
            return this;
        }

        public MockGameService MockGetTable()
        {
            Setup(m => m.GetGame(
                It.IsAny<GameDTO>()))
                .ReturnsAsync((GameDTO)null);

            Setup(m => m.GetGame(
                It.Is<GameDTO>(g => g.Id == ControllerDataToUse.GameDTO.Id)))
                .ReturnsAsync(ControllerDataToUse.GameDTO);
            return this;
        }

        public MockGameService MockCreateGame()
        {
            Setup(m => m.CreateNewGame(
                It.IsAny<GameDTO>()))
                .ReturnsAsync((GameDTO)null);

            Setup(m => m.CreateNewGame(
                It.Is<GameDTO>(g => g.Opponents.First().UserName == ControllerDataToUse.UserDTO.UserName)))
                .ReturnsAsync(ControllerDataToUse.GameDTO);
            return this;
        }

        public MockGameService MockDeleteGame()
        {
            Setup(m => m.DeteteGame(
                It.IsAny<GameDTO>()))
                .ReturnsAsync(new OperationDetails(false));

            Setup(m => m.DeteteGame(
                It.Is<GameDTO>(t => t.Id == ControllerDataToUse.GameDTO.Id)))
                .ReturnsAsync(new OperationDetails(true));
            return this;
        }       

        public MockGameService MockGetFiguresOnTable()
        {
            Setup(m => m.GetFiguresOnTable(
                It.IsAny<GameDTO>()))
                .ReturnsAsync((IEnumerable<FigureDTO>)null);

            Setup(m => m.GetFiguresOnTable(
                It.Is<GameDTO>(g => g.Id == ControllerDataToUse.GameDTO.Id)))
                .ReturnsAsync(new List<FigureDTO> { ControllerDataToUse.FigureDTO });
            return this;
        }

        public MockGameService MockJoinGame()
        {
            Setup(m => m.JoinGame(
                It.IsAny<UserDTO>(),
                It.IsAny<GameDTO>()))
                .ReturnsAsync(new OperationDetails(false));

            Setup(m => m.JoinGame(
                It.Is<UserDTO>(u => u.UserName == ControllerDataToUse.UserDTO.UserName && ControllerDataToUse.GameDTO.Opponents.Count != 2),
                It.Is<GameDTO>(g => g.Id == ControllerDataToUse.GameDTO.Id)))
                .ReturnsAsync(new OperationDetails(true)).Callback<UserDTO, GameDTO>((u, g) => ControllerDataToUse.GameDTO.Opponents.Add(u.Clone()));
            return this;
        }
    }
}
