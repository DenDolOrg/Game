using MyGame.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Interfaces
{
    public interface IGameManager : IManager<Game>
    {
        /// <summary>
        /// Finds and returns game with id like in parameter.
        /// </summary>
        /// <param name="id">Id of game to return.</param>
        /// <returns>Game with id in parameter.</returns>
        Task<Game> FindByIdAsync(int id);

        /// <summary>
        /// Returns all games from DB.
        /// </summary>
        /// <returns>List of <see cref="Game"/>.</returns>
        IQueryable<Game> GetAllGames();

        /// <summary>
        /// Returns user games.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns> List of games.</returns>
        IQueryable<Game> GetGamesForUser(int userId);

        /// <summary>
        /// Returns games user can join.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>List of games.</returns>
        IQueryable<Game> GetAvailableGames(int userId);

        /// <summary>
        /// Adding user to gamel like an opponent.
        /// </summary>
        /// <param name="gameId">Game id.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Success status of operation.</returns>
        Task<Game> AddOpponentToGame(int gameId, ApplicationUser user);

        Task<bool> TurnChange(int gameId, int userId);
    }
}
