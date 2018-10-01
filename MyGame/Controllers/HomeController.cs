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
        public ActionResult GuestIndex()
        {
            if(Session["VisitorName"] == null)
            {
                ViewBag.VisitorName = "Guest";
            }
            else
            {
                ViewBag.VisitorName = Session["VisitorName"];
            }
            
            return View();
        }
        public ActionResult PlayersIndex(Player loggedPalyer)
        {
            Session["VisitorName"] = loggedPalyer.Name + " " + loggedPalyer.Surname;
            Session["VisitorId"] = loggedPalyer.Id;
            ViewBag.VisitorName = Session["VisitorName"];
            return View();
        }

        [Authorize]
        public string Some()
        {
            return "Autorisation Works";
        }
    }
}