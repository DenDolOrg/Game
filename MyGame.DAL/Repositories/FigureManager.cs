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
    public class FigureManager : IFigureManager
    {

        /// <summary>
        /// Database context.
        /// </summary>
        public ApplicationContext Database { get; private set; }

        public FigureManager(ApplicationContext db)
        {
            Database = db;
        }

        #region CREATE
        public async Task<bool> CreateAsync(int tableId)
        {
            Table table = await Database.Tables.FindAsync(tableId);
            if (table == null)
                return false;

            try
            {
                IEnumerable<Figure> figures = SetFigures(table);
                Database.Figures.AddRange(figures);
                await Database.SaveChangesAsync();

                table.Figures = new List<Figure>(figures);
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
        public async Task<bool> DeleteAsync(int tableId)
        {
            IEnumerable<Figure> tableFigures = Database.Figures.Where(f => f.Table.Id == tableId);
            if (tableFigures == null)
                return false;

            try
            {
                foreach (Figure f in tableFigures)
                {
                    Database.Figures.Remove(f);
                }
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
 
        }
        #endregion

        #region HELPERS
        /// <summary>
        /// Set figures start position.
        /// </summary>
        private IEnumerable<Figure> SetFigures(Table table)
        {
            List<Figure> figures = new List<Figure>();
            Colors color = Colors.Black;
            int y0 = 1;
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 20; i++)
                {
                    int xCoord;
                    int yCoord = i / 5;

                    if (yCoord % 2 == 1)
                        xCoord = 2 * (i - yCoord * 5) + 1;
                    else
                        xCoord = 2 * (i - yCoord * 5 + 1);

                    figures.Add(new Figure
                    {
                        Color = color,
                        Table = table,
                        X = xCoord,
                        Y = yCoord + y0
                    });
                }
                y0 = 7;
                color = Colors.White;
            }

            return figures;
        }
        #endregion


        public IQueryable<Figure> GetFiguresForTable(int tableId)
        {
            IQueryable<Figure> tableFigures = Database.Figures.Where(f => f.Table.Id == tableId);

            return tableFigures;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
