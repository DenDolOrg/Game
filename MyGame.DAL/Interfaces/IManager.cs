using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.DAL.Entities;

namespace MyGame.DAL.Interfaces
{
    public interface IManager<T> :IDisposable
    {
        /// <summary>
        /// Creates new item of type <see cref="T"/> and adds it to DB.
        /// </summary>
        /// <param name="item">Item to add.</param>
        void Create(T item);

        /// <summary>
        /// Delete item of type <see cref="T"/> from DB.
        /// </summary>
        /// <param name="item">Item to delete.</param>
        void Delete(T item);
    }
}
