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
using Ninject;
using MyGame.Infrastructure;

namespace MyGame.Controllers
{
    /// <summary>
    /// Controller for home page
    /// </summary>
    /// <seealso cref="Controller"/>
    public class HomeController : Controller
    {
        #region SERVICES
        /// <summary>
        /// Factory for creating services.
        /// </summary>
        private ServiceFactory serviceFactory;

        /// <summary>
        /// Service which contains methods to work with users.
        /// </summary>
        private IUserService UserService
        {
            get
            {
                return serviceFactory.CreateUserService();
            }
        }

        /// <summary>
        /// Service which contains methods to work with tables.
        /// </summary>
        private ITableService TableService
        {
            get
            {
                return serviceFactory.CreateTableService();
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="HomeController"/> with default services.
        /// </summary>
        public HomeController()
        {
            serviceFactory = new HttpContextServicesFactory(
                () => HttpContext.GetOwinContext().Get<IUserService>(),
                () => HttpContext.GetOwinContext().Get<ITableService>(),
                () => null);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="HomeController"/> with custom services(for unit testing).
        /// </summary>
        public HomeController(IUserService userService, ITableService tableService, IAuthenticationManager authenticationManager)
        {
            serviceFactory = new CustomServicesFactory(userService, tableService, authenticationManager);
        }
        #endregion

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