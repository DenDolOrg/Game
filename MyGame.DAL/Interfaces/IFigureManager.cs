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
        /// <summary>
        /// Returns figures for game;
        /// </summary>
        /// <param name="gameId">Id of game.</param>
        /// <returns>List of <see cref="Figure"/></returns>
        IQueryable<Figure> GetFiguresForTable(int gameId);

        /// <summary>
        /// Returns figure with some id.
        /// </summary>
        /// <param name="figureId">Figure's id.</param>
        /// <returns><see cref="Figure"/></returns>
        Task<Figure> FindByIdAsync(int figureId);

    }
}
