using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyGame.Models;
using MyGame.Models.Abstract;

namespace MyGame.Controllers
{
    /// <summary>
    /// Controller for home page
    /// </summary>
    /// <seealso cref="Controller"/>
    public class HomeController : Controller
    {
        /// <summary>
        /// Players storage
        /// </summary>
        private IPlayerRepository _playerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        /// <param name="playerRepository">Players repository from Data Base</param>
        public HomeController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        /// <summary>
        /// Default action  of <see cref="HomeController"/>
        /// </summary>
        /// <returns>Returns a view whith the name of "<c>Views/Home/Index.cshtml</c>"</returns>
        public ActionResult Index()
        {
            //Login login = new Login()
            //{
            //    Nickname = "Sined",
            //    Email = "email@hmail.com",
            //    PasswordHash = PasswordHelper.HashPassword("myPassword"),
            //    PasswordSalt = PasswordHelper.Salt
            //};

            //Player newPlayer = new Player()
            //{
            //    Name = "Denis",
            //    Surname = "Dolich",
            //    Login = login
            //};
            //newPlayer.Login.Player = newPlayer;
            //_playerRepository.AddPlayer(newPlayer);

              //_playerRepository.RemovePlayer(1);
            return View();
        }
    }
}