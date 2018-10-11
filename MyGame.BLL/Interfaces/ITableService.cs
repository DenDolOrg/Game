using MyGame.BLL.DTO;
using MyGame.BLL.Infrastructure;
using MyGame.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.Interfaces
{
    public interface ITableService : IDisposable
    {
        /// <summary>
        /// Creates new table and figures in DB. 
        /// </summary>
        /// <param name="firstPlayer">First players information.</param>
        /// <returns>Returns table creation operation status like <see cref="OperatingSystem"/> instance.</returns>
        Task CreateNewTable(UserDTO firstPlayer);

        /// <summary>
        /// Deletes table with id = <c>tableId</c> from DB.
        /// </summary>
        /// <param name="tableId">Id of table to detele.</param>
        /// <returns>Returns table deletion operation status like <see cref="OperatingSystem"/> instance.</returns>
        Task DeteteTable(int tableId);

        /// <summary>
        /// Method to get figures information .
        /// </summary>
        /// <param name="tableId">Id of table where figures are.</param>
        /// <returns>List of universal figure data model. Each of it contains Id, X coordinate, Y coordinate of figure.</returns>
        Task<IEnumerable<FigureDTO>> GetFiguresOnTable(int tableId);

        /// <summary>
        /// Get table inforamtion with id = <c>tableId</c>.
        /// </summary>
        /// <param name="tableId">Id of table.</param>
        /// <returns>Universal table data model. Contains Id of table and list of opponents.</returns>
        TableDTO GetTable(int tableId);

        /// <summary>
        /// Returns all tables information from DB.
        /// </summary>
        /// <returns>List of universal table data model. Each of it contains Id of table and list of opponents.</returns>
        Task<IEnumerable<TableDTO>> GetAllTables();

        /// <summary>
        /// Get tables of user.
        /// </summary>
        /// <returns>List of tables.</returns>
        Task<IEnumerable<TableDTO>> GetUserTables(UserDTO user);

        /// <summary>
        /// Get available to join tables.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TableDTO>> GetAvailableTables(UserDTO userDTO);
    }
}
