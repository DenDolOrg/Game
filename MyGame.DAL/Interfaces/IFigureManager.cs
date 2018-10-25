using MyGame.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Interfaces
{
    public interface IFigureManager : IManager<int>
    {
        IQueryable<Figure> GetFiguresForTable(int gameId);

    }
}
