using MyGame.BLL.DTO;
using MyGame.BLL.Infrastructure;
using MyGame.BLL.Services;
using MyGame.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.Interfaces
{
    public interface IGameService : IDisposable
    {
        /// <summary>
        /// Creates new table and figures in DB. 
        /// </summary>
        /// <param name="firstPlayer">First players information.</param>
        /// <returns>Returns table creation operation status like <see cref="OperatingSystem"/> instance.</returns>
        Task<GameDTO> CreateNewGame(GameDTO gameDTO);

        /// <summary>
        /// Deletes table with id = <c>tableId</c> from DB.
        /// </summary>
        /// <param name="tableId">Id of table to detele.</param>
        /// <returns>Returns table deletion operation status like <see cref="OperatingSystem"/> instance.</returns>
        Task<OperationDetails> DeteteGame(GameDTO gameDTO);

        /// <summary>
        /// Deletes all user tables.
        /// </summary>
        /// <param name="user">User for which tables have to be deleted</param>
        Task<OperationDetails> DeteteUserGame(UserDTO user);

        /// <summary>
        /// Method to get figures information .
        /// </summary>
        /// <param name="tableId">Id of table where figures are.</param>
        /// <returns>List of universal figure data model. Each of it contains Id, X coordinate, Y coordinate of figure.</returns>
        Task<IEnumerable<FigureDTO>> GetFiguresOnTable(GameDTO gameDTO);

        /// <summary>
        /// Get table inforamtion with id = <c>tableId</c>.
        /// </summary>
        /// <param name="tableId">Id of table.</param>
        /// <returns>Universal table data model. Contains Id of table and list of opponents.</returns>
        Task<GameDTO> GetGame(GameDTO tableDTO);

        /// <summary>
        /// Returns all tables information from DB.
        /// </summary>
        /// <returns>List of universal table data model. Each of it contains Id of table and list of opponents.</returns>
        Task<IEnumerable<GameDTO>> GetAllGames();

        /// <summary>
        /// Get tables of user.
        /// </summary>
        /// <returns>List of tables.</returns>
        Task<IEnumerable<GameDTO>> GetUserGames(UserDTO user);

        /// <summary>
        /// Get available to join tables.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GameDTO>> GetAvailableGames(UserDTO userDTO);

        /// <summary>
        /// Changing figure position on the table. 
        /// </summary>
        /// <param name="figureData">Id of figure to change and new coords.<param>
        Task<OperationDetails> ChangeFigurePos(FigureDTO figureData);

        /// <summary>
        /// Changing turn priority. 
        /// </summary>
        /// <param name="figureData">Id of figure to change and new coords.<param>
        Task<OperationDetails> ChangeTurnPriority(GameDTO gameDTO);

        /// <summary>
        /// Adding user to the game.
        /// </summary>
        /// <param name="userDTO">User info.</param>
        /// <param name="gameDTO">Game info.</param>
        Task<OperationDetails> JoinGame(UserDTO userDTO, GameDTO gameDTO);

        /// <summary>
        /// Deletes figures from DB by id.
        /// </summary>
        /// <param name="figureDTO">Figure information class, which contains figure ids.</param>
        /// <returns>Detelion result</returns>
        Task<OperationDetails> DeleteFigures(ICollection<FigureDTO> figureDTOs);

    }
}
