using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using MyGame.Models;
using MyGame.BLL.DTO;
using System.Security.Claims;
using MyGame.BLL.Interfaces;
using MyGame.BLL.Infrastructure;
using MyGame.Infrastructure;

namespace MyGame.Controllers
{
    /// <summary>
    /// Controller for managing account.
    /// </summary>
    public class AccountController : Controller
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
        private IGameService GameService
        {
            get
            {
                return serviceFactory.CreateGameService();
            }
        }


        /// <summary>
        /// Initialises new instance of <see cref="IAuthenticationManager"/> for managing authentication process.
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return serviceFactory.CreateAuthenticationManager();
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="AccountController"/> with default services.
        /// </summary>
        public AccountController()
        {
            serviceFactory = new HttpContextServicesFactory(
                () => HttpContext.GetOwinContext().Get<IUserService>(),
                () => HttpContext.GetOwinContext().Get<IGameService>(),
                () => HttpContext.GetOwinContext().Authentication);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="AccountController"/> with custom services(for unit testing).
        /// </summary>
        public AccountController(IUserService userService, IGameService gameService = null, IAuthenticationManager authenticationManager = null)
        {
            serviceFactory = new CustomServicesFactory(userService, gameService, authenticationManager);
        }
        #endregion

        #region LOGIN(GET)
        /// <summary>
        /// Action for redirect user to the Login page.
        /// </summary>
        /// <returns>Returns view <c>Views/Account/Login.cshtml</c></returns>
        public ActionResult Login()
        {
            return View();
        }
        #endregion

        #region LOGIN(POST)
        /// <summary>
        /// Action for processing user login information.
        /// </summary>
        /// <param name="loginModel">Model ot type <see cref="LoginModel"/> which contains user's login information:
        /// <list type="number">
        /// <item>Email or Nickname;</item>
        /// <item>Password.</item>
        /// </list>
        /// </param>
        /// <returns>Redirects authorised user to home page, or displaying errors occurred in authorisation process.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = loginModel.Email, Password = loginModel.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Invalid login or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginModel);
        }
        #endregion

        #region REGISTER(GET)
        /// <summary>
        /// Action for redirect user to the Register page.
        /// </summary>
        /// <returns>Returns view <c>Views/Account/Register.cshtml</c></returns>
        public ActionResult Register()
        {           
            return View();
        }
        #endregion

        #region REGISTER(POST)
        /// <summary>
        /// Action for processing user register information.
        /// </summary>
        /// <param name="registerModel">Model of type <see cref="RegisterModel"/> which contains user's register information:
        /// <list type="number">
        /// <item>Name</item>
        /// <item>Surname</item>
        /// <item>Nickname</item>
        /// <item>Email</item>
        /// <item>Password.</item>
        /// </list>
        /// </param>
        /// <returns>Redirects authorised user to home page, or displaying errors occurred in authorisation process.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = registerModel.Email,
                    Password = registerModel.Password,
                    Name = registerModel.Name,
                    Surname = registerModel.Surname,
                    UserName = registerModel.Nickname,
                    Role = "user"
                };
                OperationDetails operationDetails = await UserService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = await UserService.Authenticate(userDto);
                    AuthenticationManager.SignIn();
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(operationDetails.PropErrorName, operationDetails.ErrorMessage);
            }
            return View(registerModel);
        }
        #endregion

        #region LOGOUT
        /// <summary>
        /// Action for processing logout event. Redirects to the home page;
        /// </summary>
        /// <returns>Returns view <c>Views/Home/Index</c></returns>
        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Action for processing user deletion event.
        /// </summary>
        /// <param name="email">Email of user to delete.</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        public async Task Delete(int id)
        {
            UserDTO userDTO = new UserDTO{ Id = id };

            var userTableDelResult = await GameService.DeteteUserGame(userDTO);
            var userDelResult = await UserService.Delete(userDTO);

            if (!(userDelResult.Succedeed && userTableDelResult.Succedeed))
                throw new HttpException(409, userTableDelResult.ErrorMessage + " " + userDelResult.ErrorMessage);
        }
        #endregion

        #region USER_LIST
        /// <summary>
        /// Returns view with list of users.
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            return View();
        }
        #endregion

        #region GET_ALL_USERS
        /// <summary>
        /// Returns user list.
        /// </summary>
        /// <returns>Json object for user list.</returns>
        public async Task<JsonResult> GetAllUsers()
        {
            IEnumerable<UserDTO> users = await UserService.GetAllUsers();

            return Json(users, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
