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
        public async Task<bool> CreateAsync(int gameId)
        {
            Game game = await Database.Games.FindAsync(gameId);         
            if (game == null)
                return false;
            try
            {
                Database.Figures.AddRange(SetFigures(game));
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
        public async Task<bool> DeleteAsync(int gameId)
        {
            IEnumerable<Figure> tableFigures = Database.Figures.Where(f => f.Game.Id == gameId);
            if (tableFigures == null)
                return false;

            try
            {
                foreach (Figure f in tableFigures)
                    Database.Figures.Remove(f);

                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region FIGURES_ON_TABLE
        public IQueryable<Figure> GetFiguresForTable(int gameId)
        {
            IQueryable<Figure> tableFigures = Database.Figures.Where(f => f.Game.Id == gameId);

            return tableFigures;
        }
        #endregion
        #region HELPERS
        /// <summary>
        /// Set figures start position.
        /// </summary>
        private ICollection<Figure> SetFigures(Game game)
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
                        Game = game,
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




        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
