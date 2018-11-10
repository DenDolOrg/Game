using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.DAL.Entities;
using MyGame.DAL.Identity;
namespace MyGame.DAL.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        /// <summary>
        /// User manager.
        /// </summary>
        /// <seealso cref="ApplicationUserManager"/>
        ApplicationUserManager UserManager { get; }

        /// <summary>
        /// Role manager.
        /// </summary>
        /// <seealso cref="ApplicationRoleManager"/>
        ApplicationRoleManager RoleManager { get; }

        /// <summary>
        /// Player manager.
        /// </summary>
        IPlayerManager PlayerManager { get; }

        /// <summary>
        /// Table manager.
        /// </summary>
        IGameManager GameManager { get; }

        /// <summary>
        /// Table manager.
        /// </summary>
        ITableManager TableManager { get; }

        /// <summary>
        /// Figure manager.
        /// </summary>
        IFigureManager FigureManager { get; }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        Task SaveChangesAsync();

    }
}
