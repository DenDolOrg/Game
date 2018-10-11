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
    class TableManager : ITableManager
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

        public async Task CreateAsync(Table item)
        {
            Database.Tables.Add(item);
            await Database.SaveChangesAsync();
        }

        public async Task DeleteAsync(Table item)
        {

            Database.Tables.Remove(item);
            await Database.SaveChangesAsync();
        }

        public Table FindById(int id)
        {
            return Database.Tables.Find(id);
        }

        public IQueryable<Table> GetAllTabes()
        {
            IQueryable<Table> tables  = Database.Tables;
            return tables;
        }

        public IQueryable<Table> GetTablesForUser(int userId)
        {
            IQueryable<Table> tables = Database.Tables.Where(t => t.Opponents.Where(o => o.Id == userId).Count() > 0);

            return tables;
        }

        public IQueryable<Table> GetAvailableTables(int userId)
        {
            IQueryable<Table> tables = Database.Tables.Where(t => (t.Opponents.Count == 1) && (t.Opponents.Where( o => o.Id == userId).Count() == 0));

            return tables;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
