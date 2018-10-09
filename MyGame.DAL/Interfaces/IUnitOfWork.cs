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
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IPlayerManager PlayerManager { get; }
        ITableManager TableManager { get; }
        IFigureManager FigureManager { get; }
        Task SaveAsync();

    }
}
