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
        public void Create(int tableId)
        {
            Table table = Database.Tables.Find(tableId);
            ICollection<Figure> figures = SetFigures(table);
            Database.Figures.AddRange(figures.AsEnumerable());
            Database.SaveChangesAsync().Wait();
            figures.CopyTo(table.Figures.ToArray(), 0);
            Database.SaveChangesAsync().Wait();
        }

        public void Delete(int tableId)
        {
            if (tableId > 0)
            {
                IEnumerable<Figure> tableFigures = Database.Figures.Where(f => f.Table.Id == tableId);
                foreach(Figure f in tableFigures)
                {
                    Database.Figures.Remove(f);
                }
                
            Database.SaveChanges();

            }
        }
        public void Dispose()
        {
            Database.Dispose();
        }

        /// <summary>
        /// Set figures start position.
        /// </summary>
        private ICollection<Figure> SetFigures(Table table)
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

        public IEnumerable<Figure> GetFiguresForTable(int tableId)
        {
            IEnumerable<Figure> tableFigures = Database.Figures.Where(f => f.Table.Id == tableId);

            return tableFigures;
        }
    }
}
