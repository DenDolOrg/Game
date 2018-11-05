using MyGame.BLL.DTO;
using MyGame.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyGame.Tests.Models
{
    internal static class ControllerDataToUse
    {
        public static UserDTO UserDTO { get; set; }

        public static FigureDTO FigureDTO { get; set; }

        public static GameDTO GameDTO { get; set; }

        public static UserDTO Clone(this UserDTO user)
        {
            var clone = new UserDTO
            {
                Id = UserDTO.Id,
                Email = UserDTO.Email,
                Name = UserDTO.Name,
                Surname = UserDTO.Surname,
                UserName = UserDTO.UserName,
                Password = UserDTO.Password
            };

            return clone;
        }

        public static void SetData()
        {
            UserDTO = new UserDTO
            {
                Id = 1,
                Email = "email",
                Name = "name",
                Surname = "surname",
                Password = "password",
                UserName = "username"
            };

            GameDTO = new GameDTO
            {
                Id = 1,
                BlackPlayerId = 1,
                Opponents = new List<UserDTO>
                {
                    UserDTO.Clone()
                }
            };

            FigureDTO = new FigureDTO
            {
                Id = 1,
                Color = "black",
                XCoord = 1,
                YCoord = 1,
            };
        }
    }
}
