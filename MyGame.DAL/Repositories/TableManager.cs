using MyGame.DAL.Entities;
using MyGame.DAL.EntityFramework;
using MyGame.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Repositories
{
    public class TableManager : ITableManager
    {
        /// <summary>
        /// Database context.
        /// </summary>
        public ApplicationContext Database { get; private set; }

        /// <summary>
        /// Initializes a new instance of<see cref= "TableManager" />.
        /// </summary>
        /// <param name="db">Database context.</param>
        public TableManager(ApplicationContext db)
        {
            Database = db;
        }

        #region CREATE
        public async Task<bool> CreateAsync(Table item)
        {
            try
            {
                Database.Tables.Add(item);
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion

        #region DELETE
        public async Task<bool> DeleteAsync(Table item)
        {
            try
            {
                Database.Tables.Remove(item);
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region FIND_BY_ID
        public async Task<Table> FindByIdAsync(int id)
        {
            return await Database.Tables.FindAsync(id);
        }
        #endregion

        #region GET_ALL_TABLES
        public IQueryable<Table> GetAllTabes()
        {
            IQueryable<Table> tables  = Database.Tables;
            return tables;
        }
        #endregion

        #region GET_USER_TABLES
        public IQueryable<Table> GetTablesForUser(int userId)
        {
            IQueryable<Table> tables = Database.Tables.Where(t => t.Opponents.Where(o => o.Id == userId).Count() > 0);

            return tables;
        }
        #endregion

        #region GET_AVAILABLE_TABLES
        public IQueryable<Table> GetAvailableTables(int userId)
        {
            IQueryable<Table> tables = Database.Tables.Where(t => (t.Opponents.Count == 1) && (t.Opponents.Where( o => o.Id == userId).Count() == 0));

            return tables;
        }
        #endregion
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
