using MyGame.BLL.DTO;
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
                    CreationTime = newGame.Table.CreationTime.ToShortTimeString()
                };

            }
            return null;
        }
        #endregion

        #region DELETE
        public async Task<OperationDetails> DeteteGame(GameDTO tableDTO)
        {
            OperationDetails successOD = new OperationDetails(true);
            OperationDetails failOD = new OperationDetails(false);

            Game game = await Database.GameManager.FindByIdAsync(tableDTO.Id);
            if (game == null)
                return failOD;

            bool figuresDelResult = await Database.FigureManager.DeleteAsync(game.Id);
            bool tableDelResult = await Database.TableManager.DeleteAsync(game);
            bool gameDelResult = await Database.GameManager.DeleteAsync(game);

            if (!(figuresDelResult && tableDelResult && gameDelResult))
                return failOD;

            return successOD;


        }
        #endregion

        #region DELETE_USER_GAME
        public async Task<OperationDetails> DeteteUserGame(UserDTO userDTO)
        {
            OperationDetails successOD = new OperationDetails(true);
            OperationDetails failOD = new OperationDetails(false);

            ApplicationUser user = await Database.UserManager.FindByIdAsync(userDTO.Id);

            if (user == null)
                return failOD;

            if (user.Games == null)
                return successOD;

            IEnumerable<int> games = user.Games.Select(g => g.Id);

            foreach (int id in games)
            {
                var GameDelResult = await DeteteGame(new GameDTO { Id = id });
                if (!GameDelResult.Succedeed)
                    return failOD;
            }

            return successOD;
        }
        #endregion

        #region GET_FIGURES
        public async Task<IEnumerable<FigureDTO>> GetFiguresOnTable(GameDTO gameDTO)
        {
            ICollection<Figure> tableFigures = await Database.FigureManager.GetFiguresForTable(gameDTO.Id).ToListAsync();
            if (tableFigures.Count() != 0)
            {
                return CreateFiguresDTO(tableFigures);
            }
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

            return CreateGameDTO(gameList);
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
            OperationDetails failOD = new OperationDetails(false);

            Figure figure = await Database.FigureManager.FindByIdAsync(figureData.Id);
            if (figure == null)
                return failOD;

            figure.X = figureData.XCoord;
            figure.Y = figureData.YCoord;

            try
            {
                await Database.SaveChangesAsync();
            }
            catch
            {
                return failOD;
            }

            return successOD;
        }
        #endregion

        #region JOIN_TO_TABLE

        public async Task<OperationDetails> JoinGame(UserDTO userDTO, GameDTO gameDTO)
        {
            OperationDetails successOD = new OperationDetails(true);
            OperationDetails failOD = new OperationDetails(false);

            if (userDTO == null || gameDTO == null)
                return failOD;

            var user = await Database.UserManager.FindByNameAsync(userDTO.UserName);
            if (user == null)
                return failOD;

            var game = await Database.GameManager.AddOpponentToGame(gameDTO.Id, user.Id);

            if (game == null)
                return failOD;

            if (game.WhitePlayerId == 0 && game.BlackPlayerId != user.Id)
                game.WhitePlayerId = user.Id;
            else if(game.BlackPlayerId == 0 && game.WhitePlayerId != user.Id)
                game.BlackPlayerId = user.Id;

            gameDTO.WhitePlayerId = game.WhitePlayerId;
            gameDTO.BlackPlayerId = game.BlackPlayerId;

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
                    CreationTime = g.Table.CreationTime.ToShortDateString()
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
