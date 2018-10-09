using MyGame.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Interfaces
{
    public interface ITableManager : IManager<Table>
    {
        /// <summary>
        /// Finds and returns table with id like in parameter.
        /// </summary>
        /// <param name="id">Id of table to return.</param>
        /// <returns>Table with id in parameter.</returns>
        Table FindById(int id);

        /// <summary>
        /// Returns all tables from DB.
        /// </summary>
        /// <returns>List of <see cref="Table"/>.</returns>
        IEnumerable<Table> GetAllTabes();

        IEnumerable<Table> GetTablesForUser(int userId);


    }
}
