using MyGame.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.Models
{
    internal static class ServiceDataToUse
    {
        public static Game Game { get; set; }
        public static ApplicationUser User { get; set; }
        public static Table Table { get; set; }
        public static Figure Figure { get; set; }

        public static void SetData()
        {
            SetUser();
            SetTable();
            SetFigure();
            SetGame();
        }

        private static void SetGame()
        {
            Game game = new Game
            {
                Id = 1,
                Table = Table
            };

            game.Opponents.Add(User);
            Game = game;
            Figure.Game = Game;
        }
        private static void SetUser()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = 1,
                Email = "email@gmail.com",
                PlayerProfile = new PlayerProfile
                {
                    Id = 1,
                    Name = "name",
                    Surname = "surname"
                },
                PasswordHash = "password",               
            };

            User = user;
        }

        private static void SetTable()
        {
            Table table = new Table
            {
                Id = 1
            };

            Table = table;
        }

        private static void SetFigure()
        {
            Figure figure = new Figure()
            {
                Id = 1,
                Color = Colors.Black,
                X = 1,
                Y = 1,
            };

            Figure = figure;
        }
    }
}
