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
        void CreateNewTable(UserDTO firstPlayer);

        /// <summary>
        /// Deletes table with id = <c>tableId</c> from DB.
        /// </summary>
        /// <param name="tableId">Id of table to detele.</param>
        /// <returns>Returns table deletion operation status like <see cref="OperatingSystem"/> instance.</returns>
        void DeteteTable(int tableId);

        /// <summary>
        /// Method to get figures information .
        /// </summary>
        /// <param name="tableId">Id of table where figures are.</param>
        /// <returns>List of universal figure data model. Each of it contains Id, X coordinate, Y coordinate of figure.</returns>
        IEnumerable<FigureDTO> GetFiguresOnTable(int tableId);

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
        IEnumerable<TableDTO> GetAllTables();

        /// <summary>
        /// Get table for user.
        /// </summary>
        /// <returns></returns>
        ICollection<TableDTO> GetTablesForUser(UserDTO user);
    }
}
