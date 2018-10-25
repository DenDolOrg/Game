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
        public async Task<bool> CreateAsync(Game game)
        {
            Table newTable = new Table { Id = game.Id };
            try
            {
                Database.Tables.Add(newTable);
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(Game game)
        {
            try
            {
                if(game.Table != null)
                    Database.Tables.Remove(game.Table);
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

       
    }
}
