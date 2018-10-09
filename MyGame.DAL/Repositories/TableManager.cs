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

        public void Create(Table item)
        {
            Database.Tables.Add(item);
            Database.SaveChanges();
        }

        public void Delete(Table item)
        {
            Database.Tables.Remove(item);
            Database.SaveChanges();
        }

        public Table FindById(int id)
        {
            return Database.Tables.Find(id);
        }

        public IEnumerable<Table> GetAllTabes()
        {
            return Database.Tables.AsEnumerable();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<Table> GetTablesForUser(int userId)
        {
            IEnumerable<Table> tables = Database.Tables.Where(t => t.Opponents.Where(o => o.Id == userId).Count() > 0);
            return tables;
        }
    }
}
