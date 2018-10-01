using MyGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyGame.Models.Abstract;

namespace MyGame.Controllers
{
    public class AutorisationController : Controller
    {
        private IPlayerRepository _playerRepository;
        private ILoginRepository _loginRepository;
        public AutorisationController(IPlayerRepository playerRepository, ILoginRepository loginRepository)
        {
            _playerRepository = playerRepository;
            _loginRepository = loginRepository;
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(Player newPlayer)
        {
            newPlayer.Login.PasswordHash = PasswordHelper.HashPassword(newPlayer.Login.PasswordHash);
            newPlayer.Login.Player = newPlayer;

            _playerRepository.AddPlayer(newPlayer);
            return RedirectToAction("GuestIndex", "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login)
        {
            Login loginFromDB = _loginRepository.LoginList.FirstOrDefault(l => l.Email == login.Email);
            if(loginFromDB == null || !PasswordHelper.VerifyHashedPassword(loginFromDB.PasswordHash, login.PasswordHash))
            {
                ViewBag.ErrorMessage = "Check your email or password";
                return View();
            }
            Player loggedPlayer = _playerRepository.PlayerList.First(p => p.Id == loginFromDB.Id);
            return RedirectToAction("PlayersIndex", "Home", loggedPlayer);
        }
    }
}
