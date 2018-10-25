using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyGame.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyGame.BLL.DTO;
using System.Threading.Tasks;
using MyGame.Models;
using MyGame.Infrastructure;
using MyGame.BLL.Infrastructure;

namespace MyGame.Controllers
{
    [Authorize]
    public class GameController : Controller
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
        /// Initialises a new instance of <see cref="GameController"/> with default services.
        /// </summary>
        public GameController()
        {
            serviceFactory = new HttpContextServicesFactory(
                () => HttpContext.GetOwinContext().Get<IUserService>(),
                () => HttpContext.GetOwinContext().Get<IGameService>(),
                () => null);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="GameController"/> with custom services(for unit testing).
        /// </summary>
        public GameController(IUserService userService, IGameService gameService = null, IAuthenticationManager authenticationManager = null)
        {
            serviceFactory = new CustomServicesFactory(userService, gameService, authenticationManager);
        }
        #endregion

        #region GAME_LIST
        /// <summary>
        /// Decides which list to show.
        /// </summary>
        /// <param name="gameType">Table type to show.</param>
        /// <returns>View with needed list.</returns>
        [Authorize]
        public ViewResult GameList(string gameType)
        {
            TableActionModel tableAction = new TableActionModel();
            if (gameType == "all")
                tableAction.ActionName = "/Game/GetAllGames";

            else if (gameType == "available")
                tableAction.ActionName = "/Game/GetAvailableGames";

            else if (gameType == "myGames")
                tableAction.ActionName = "/Game/GetUserGames";
            else
                return null;

            return View(tableAction);
        }
        #endregion

        #region USER_GAMES
        /// <summary>
        /// Returns tables where current user is one of the opponents.
        /// </summary>
        /// <returns>Json object for table list.</returns>
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetUserGames()
        {
            UserDTO userDTO = new UserDTO { UserName = HttpContextManager.Current.User.Identity.Name };

            IEnumerable<GameDTO> tables = await GameService.GetUserGames(userDTO);

            if(tables != null)
                return Json(tables, JsonRequestBehavior.AllowGet);

            return null;
        }
        #endregion

        #region ALL_GAMES

        /// <summary>
        /// Returns all table.
        /// </summary>
        /// <returns>Json object for table list.</returns>

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<JsonResult> GetAllGames()
        {
            IEnumerable<GameDTO> games = await GameService.GetAllGames();

            return Json(games, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AVAILABLE_TABLES
        /// <summary>
        /// Returns available to join tables.
        /// </summary>
        /// <returns>Json object for table list.</returns>
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetAvailableGames()
        {
            UserDTO userDTO = new UserDTO { UserName = HttpContextManager.Current.User.Identity.Name };

            IEnumerable<GameDTO> games = await GameService.GetAvailableGames(userDTO);

            if(games != null)
                return Json(games, JsonRequestBehavior.AllowGet);

            return null;
        }
        #endregion

        #region CREATE_TABLE
        /// <summary>
        /// Creates new table for user.
        /// </summary
        public async Task<ActionResult> CreateNewGame()
        {
            UserDTO user = new UserDTO { UserName = HttpContextManager.Current.User.Identity.Name };

            OperationDetails details = await GameService.CreateNewGame(user);
            if(details.Succedeed)
                return RedirectToAction("GameList", "Game", new { gameType = "myGames" });

            return null;
        }
        #endregion

        #region DELETE_GAME
        /// <summary>
        /// Deletes table with some id.
        /// </summary>
        /// <param name="id">Id of table to delete.</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {       
            OperationDetails details = await GameService.DeteteGame(new GameDTO {Id = id });

            if (!details.Succedeed)
                throw new HttpException(403, "Error while deleting");

        }
        #endregion
    }
}
