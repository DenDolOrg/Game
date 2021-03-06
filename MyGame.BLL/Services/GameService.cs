﻿using MyGame.BLL.DTO;
using MyGame.BLL.Infrastructure;
using MyGame.BLL.Interfaces;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.Services
{
    public class GameService : IGameService
    {

        /// <summary>
        /// Instance of <see cref="IUnitOfWork"/> which contains managers of entities and DB context;
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="UserService"/>.
        /// </summary>
        /// <param name="db">Instance of <see cref="IUnitOfWork"/></param>
        public GameService(IUnitOfWork db)
        {
            Database = db;
        }

        #region CREATE
        public async Task<GameDTO> CreateNewGame(GameDTO gameDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(gameDTO.Opponents.First().UserName);
            if (user != null)
            {
                List<ApplicationUser> opponents = new List<ApplicationUser> { user };
                var newGame = new Game
                {
                    Opponents = opponents
                };

                if (gameDTO.WhitePlayerId == 0)
                    newGame.BlackPlayerId = user.Id;
                else
                    newGame.WhitePlayerId = user.Id;

                newGame.LastTurnPlayerId = newGame.BlackPlayerId;

                bool gameCreateRes = await Database.GameManager.CreateAsync(newGame);

                bool tableCreateRes = await Database.TableManager.CreateAsync(newGame);
                bool figuresCreateRes = await Database.FigureManager.CreateAsync(newGame.Id);


                if (!(gameCreateRes && tableCreateRes && figuresCreateRes))
                    return null;

                user.Games.Add(newGame);
                try
                {
                    await Database.SaveChangesAsync();
                }
                catch
                {
                    return null;
                }

                return new GameDTO
                {
                    Id = newGame.Id,
                    Opponents = GetOpponents(newGame),
                    CreationTime = newGame.Table.CreationTime.ToShortTimeString(),
                    LastTurnPlayerId = newGame.LastTurnPlayerId,
                    BlackPlayerId = newGame.BlackPlayerId,
                    WhitePlayerId = newGame.WhitePlayerId
                };

            }
            return null;
        }
        #endregion

        #region DELETE
        public async Task<OperationDetails> DeteteGame(GameDTO tableDTO)
        {
            OperationDetails successOD = new OperationDetails(true);

            Game game = await Database.GameManager.FindByIdAsync(tableDTO.Id);
            if (game == null)
                return new OperationDetails(false, "No game with requested id.");

            bool figuresDelResult = await Database.FigureManager.DeleteAsync(game.Id);
            bool tableDelResult = await Database.TableManager.DeleteAsync(game);
            bool gameDelResult = await Database.GameManager.DeleteAsync(game);

            if (!(figuresDelResult && tableDelResult && gameDelResult))
                return new OperationDetails(false, "Failed while deleting game or table or figures.");

            return successOD;
        }
        #endregion

        #region DELETE_USER_GAME
        public async Task<OperationDetails> DeteteUserGame(UserDTO userDTO)
        {
            OperationDetails successOD = new OperationDetails(true);

            ApplicationUser user = await Database.UserManager.FindByIdAsync(userDTO.Id);

            if (user == null)
                return new OperationDetails(false, "No user with requested id.");

            if (user.Games == null)
                return successOD;

            IEnumerable<int> games = user.Games.Select(g => g.Id);

            foreach (int id in games)
            {
                var gameDelResult = await DeteteGame(new GameDTO { Id = id });
                if (!gameDelResult.Succedeed)
                    return new OperationDetails(false, gameDelResult.ErrorMessage);
            }

            return successOD;
        }
        #endregion

        #region GET_FIGURES
        public async Task<IEnumerable<FigureDTO>> GetFiguresOnTable(GameDTO gameDTO)
        {
            ICollection<Figure> tableFigures = await Database.FigureManager.GetFiguresForTable(gameDTO.Id).ToListAsync();
            if (tableFigures != null && tableFigures.Count() != 0)
                return CreateFiguresDTO(tableFigures);

            return null;
        }
        #endregion

        #region GET_GAME
        public async Task<GameDTO> GetGame(GameDTO gameDTO)
        {
            Game game = await Database.GameManager.FindByIdAsync(gameDTO.Id);
            if (game != null)
            {
                GameDTO tableDTO_out = new GameDTO
                {
                    Id = game.Id,
                    Opponents = GetOpponents(game),
                    WhitePlayerId = game.WhitePlayerId,
                    BlackPlayerId = game.BlackPlayerId,
                    LastTurnPlayerId = game.LastTurnPlayerId,
                    CreationTime = game.Table.CreationTime.ToShortDateString()
                };

                return tableDTO_out;
            }
            return null;
        }
        #endregion

        #region GET_ALL_GAMES
        public async Task<IEnumerable<GameDTO>> GetAllGames()
        {
            IEnumerable<Game> gameList = await Database.GameManager.GetAllGames().ToListAsync();
            if(gameList != null && gameList.Count() != 0)
                return CreateGameDTO(gameList);

            return null;
        }
        #endregion

        #region GET_GAMES_FOR_USER
        public async Task<IEnumerable<GameDTO>> GetUserGames(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userDTO.UserName);

            if (user == null)
                return null;

            IEnumerable<Game> games = await Database.GameManager.GetGamesForUser(user.Id).ToListAsync();

            return CreateGameDTO(games);
        }
        #endregion

        #region AVAILABLE_GAMES
        public async Task<IEnumerable<GameDTO>> GetAvailableGames(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userDTO.UserName);
            if (user == null)
                return null;

            IEnumerable<Game> games = await Database.GameManager.GetAvailableGames(user.Id).ToListAsync();

            return CreateGameDTO(games);
        }
        #endregion

        #region CHANGE_FIGURE_POSITION
        public async Task<OperationDetails> ChangeFigurePos(FigureDTO figureData)
        {
            OperationDetails successOD = new OperationDetails(true);

            Figure figure = await Database.FigureManager.FindByIdAsync(figureData.Id);
            if (figure == null)
                return new OperationDetails(false, "No figure with requested id.");

            figure.X = figureData.XCoord;
            figure.Y = figureData.YCoord;
            figure.IsSuperFigure = figureData.IsSuperFigure;

            try
            {
                await Database.SaveChangesAsync();
            }
            catch
            {
                return new OperationDetails(false, "Error while saving changes to database"); ;
            }

            return successOD;
        }
        #endregion

        #region JOIN_TO_TABLE
        public async Task<OperationDetails> JoinGame(UserDTO userDTO, GameDTO gameDTO)
        {
            OperationDetails successOD = new OperationDetails(true);

            var user = await Database.UserManager.FindByNameAsync(userDTO.UserName);
            if (user == null)
                return new OperationDetails(false, "Error while searching user.");

            var game = await Database.GameManager.AddOpponentToGame(gameDTO.Id, user);

            if (game == null)
                return new OperationDetails(false, "Error while adding user to the game.");

            if (game.WhitePlayerId == 0 && game.BlackPlayerId != user.Id)
                game.WhitePlayerId = user.Id;
            else if(game.BlackPlayerId == 0 && game.WhitePlayerId != user.Id)
                game.BlackPlayerId = user.Id;

            if (game.LastTurnPlayerId == 0 && user.Id == game.BlackPlayerId)
                game.LastTurnPlayerId = user.Id;

            gameDTO.WhitePlayerId = game.WhitePlayerId;
            gameDTO.BlackPlayerId = game.BlackPlayerId;
            gameDTO.LastTurnPlayerId = game.LastTurnPlayerId;
            gameDTO.Opponents = GetOpponents(game);

            userDTO.Id = user.Id;

            try
            {
                await Database.SaveChangesAsync();
            }
            catch
            {
                return new OperationDetails(false, "Error while saving changes to database.");
            }
            return successOD;
        }
        #endregion

        #region TURN_PRIORITY
        public async Task<OperationDetails> ChangeTurnPriority(GameDTO gameDTO)
        {
            OperationDetails successOD = new OperationDetails(true);

            var turnChangeRes = await Database.GameManager.TurnChange(gameDTO.Id, gameDTO.LastTurnPlayerId);

            if (!turnChangeRes)
                return new OperationDetails(false, "Failed while changing turn priority.");

            try
            {
                await Database.SaveChangesAsync();
            }
            catch
            {
                return new OperationDetails(false, "Error while saving changes to database.");
            }

            return successOD;
        }
        #endregion

        #region DELETE_FIGURE
        public async Task<OperationDetails> DeleteFigures(ICollection<FigureDTO> figureDTOs)
        {
            OperationDetails successOD = new OperationDetails(true);
            if (!(await Database.FigureManager.DeleteSomeFiguresAsync(figureDTOs.Select(f => f.Id))))
            {
                return new OperationDetails(false, "Failed while deleting figure.");
            }
            return successOD;

        }
        #endregion

        #region HELPERS

        /// <summary>
        /// Returns list of opponents on Table.
        /// </summary>
        /// <param name="table">Table to scan.</param>
        /// <returns>List of universal table data model. Each of it contains Id of table and list of opponents.</returns>
        private ICollection<UserDTO> GetOpponents(Game game)
        {
            List<UserDTO> opponents = new List<UserDTO>();
            foreach (ApplicationUser u in game.Opponents)
            {
                opponents.Add(new UserDTO
                {
                    Id = u.Id,
                    Name = u.PlayerProfile.Name,
                    Surname = u.PlayerProfile.Surname,
                    Email = u.Email,
                    UserName = u.UserName
                });
            }

            return opponents;
        }

        private ICollection<FigureDTO> CreateFiguresDTO(ICollection<Figure> figures)
        {
            List<FigureDTO> figureDTOs = new List<FigureDTO>();

            foreach (Figure f in figures)
            {
                figureDTOs.Add(new FigureDTO
                {
                    Id = f.Id,
                    XCoord = f.X,
                    YCoord = f.Y,
                    IsSuperFigure = f.IsSuperFigure,
                    Color = f.Color.ToString()
                });
            }
            return figureDTOs;
        }

        /// <summary>
        /// Creates list of <see cref="GameDTO"/> for list of <see cref="Table"/>
        /// </summary>
        /// <param name="games">Table list.</param>
        /// <returns>List of tableDTOs.</returns>
        private IEnumerable<GameDTO> CreateGameDTO(IEnumerable<Game> games)
        {
            List<GameDTO> gameDTOs = new List<GameDTO>();

            ICollection<UserDTO> opponents;
            for (int i = 0; i < games.Count(); i++)
            {
                Game g = games.ElementAt(i);
                opponents = GetOpponents(g);
                gameDTOs.Add(new GameDTO
                {
                    Id = g.Id,
                    Opponents = opponents,
                    CreationTime = g.Table.CreationTime.ToShortDateString(),
                    LastTurnPlayerId = g.LastTurnPlayerId,
                    BlackPlayerId =g.BlackPlayerId,
                    WhitePlayerId = g.WhitePlayerId
                });
            }
            return gameDTOs;
        }
        #endregion

        

        public void Dispose()
        {
            Database.Dispose();
        }

    }
}
