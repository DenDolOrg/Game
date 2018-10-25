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

        public static GameDTO GameDTO { get; set; }

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
                Id = 1
            };
        }
    }
}
