﻿using Microsoft.AspNet.Identity.Owin;
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
using MyGame.Real_time;
using MyGame.DAL.Entities;

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
            GameActionModel tableAction = new GameActionModel();
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

        #region CREATE_TABLE_POST
        /// <summary>
        /// Creates new table for user.
        /// </summary
        [HttpPost]
        public async Task<ActionResult> CreateNewGame(NewGameModel model)
        {
            var user = await UserService.GetUser(new UserDTO { UserName = HttpContextManager.Current.User.Identity.Name });
            if(user == null)
                throw new HttpException(503, "Unexpected error.");

            var newGameDTO = new GameDTO
            {
                Opponents = new List<UserDTO> { user },
                BlackPlayerId = model.FirstColor == "Black" ? user.Id : 0,
                WhitePlayerId = model.FirstColor == "White" ? user.Id : 0,
            };

            var createdGameDTO = await GameService.CreateNewGame(newGameDTO);
            if(createdGameDTO == null)
                throw new HttpException(503, "Unexpected error.");

            return RedirectToAction("EnterGame", new { gameId = createdGameDTO.Id});
            
        }
        #endregion

        #region CREATE_TABLE_GET
        /// <summary>
        /// Shows form for creating new game.
        /// </summary>
        /// <returns>Form view.</returns>
        [HttpGet]
        public ActionResult CreateNewGame()
        {
            return View("CreateGameForm");
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

        #region ENTER_GAME
        /// <summary>
        /// Enter game with some id.
        /// </summary>
        /// <param name="gameId">Id of game to enter.</param>
        public async Task<ActionResult> EnterGame(int gameId)
        {
            var user = await UserService.GetUser(new UserDTO { UserName = HttpContextManager.Current.User.Identity.Name });
            var game = new GameDTO { Id = gameId };

            if(user == null || game == null)
                throw new HttpException(404, "Unexpected error");

            var joinResult = await GameService.JoinGame(user, game);

            if (!joinResult.Succedeed)
                return RedirectToAction("GameList", new { gameType = "available" });

            var figures = await GameService.GetFiguresOnTable(game);
            if (figures == null)
                throw new HttpException(404, "Unexpected error");

            var gameModel = new GameModel
            {
                ThisPlayerId = user.Id,
                GameId = game.Id,
                BlackId = game.BlackPlayerId,
                WhiteId = game.WhitePlayerId,
                Figures = await GameService.GetFiguresOnTable(game),
                isMyTurn = (game.LastTurnPlayerId != user.Id),
                MyName = user.UserName

            };
            var opponentName = game.Opponents.FirstOrDefault(o => o.UserName != user.UserName);
            if (opponentName != null)
                gameModel.OpponentName = opponentName.UserName;

            return View("SingleTable", gameModel);

        }
        #endregion

        [HttpPost]
        public async Task ChangeFigurePos(StepModel model)
        {
            var changePosResult = await GameService.ChangeFigurePos(new FigureDTO
            {
                Id = int.Parse(model.FigureId),
                XCoord = int.Parse(model.NewXPos),
                YCoord = int.Parse(model.NewYPos)
            });
            var opponent = await UserService.GetUser(new UserDTO { UserName = HttpContextManager.Current.User.Identity.Name });

            if(opponent == null)
                throw new HttpException(404, "Unexpected error.");

            var changeTurnQuery = await GameService.ChangeTurnPriority(new GameDTO
            {
                Id = int.Parse(model.GameId),
                LastTurnPlayerId = opponent.Id
            });
            if (!changePosResult.Succedeed || !changeTurnQuery.Succedeed)
                throw new HttpException(404, "Unexpected error.");


        }
    }
}
