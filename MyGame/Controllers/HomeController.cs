using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyGame.Controllers
{
    /// <summary>
    /// Controller for home page
    /// </summary>
    /// <seealso cref="Controller"/>
    public class HomeController : Controller
    {
        /// <summary>
        /// Default action  of <see cref="HomeController"/>
        /// </summary>
        /// <returns>Returns a view whith the name of "<c>Views/Home/Index.cshtml</c>"</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}