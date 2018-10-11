using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using MyGame.Models;
using MyGame.BLL.DTO;
using System.Security.Claims;
using MyGame.BLL.Interfaces;
using MyGame.BLL.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MyGame.Controllers
{
    /// <summary>
    /// Controller for home page
    /// </summary>
    /// <seealso cref="Controller"/>
    public class HomeController : Controller
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController" /> class.
        /// </summary>
        public HomeController()
        {

        }
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }
        /// <summary>
        /// Returns new instance of <see cref="IAuthenticationManager"/> for managing authentication process.
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ITableService TableService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ITableService>();
            }
        }

        /// <summary>
        /// Default action  of <see cref="HomeController"/>
        /// </summary>
        /// <returns>Returns a view whith the name of "<c>Views/Home/Index.cshtml</c>"</returns>
        public async Task<ActionResult> Index()
        {
            UserDTO receivedUserDTO;
            if (Session["FullName"] == null)
            {
                receivedUserDTO =  await UserService.GetUser(new UserDTO { UserName = HttpContext.User.Identity.Name });
                if (receivedUserDTO != null)
                {
                    string fullName = receivedUserDTO.Name + " " + receivedUserDTO.Surname;
                    Session["FullName"] = fullName;
                }
            }
            return View();
        }

        public ActionResult UserHome()
        {
            return View("Index");
        }

    }
}