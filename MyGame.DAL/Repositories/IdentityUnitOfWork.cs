using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.DAL.EntityFramework;
using MyGame.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyGame.DAL.Repositories
{
    /// <summary>
    /// Class for combining and storage all managers and contexts.
    /// </summary>
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUnitOfWork" /> class.
        /// </summary>
        public IdentityUnitOfWork()
        {
            ContextFactory contextFactory = new ContextFactory();
            db = contextFactory.Create();
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser, ApplicationRole, int, UserLogin, UserRole, UserClaim>(db));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole, int, UserRole>(db));
            PlayerManager = new PlayerManager(db);
            GameManager = new GameManager(db);
            TableManager = new TableManager(db);
            FigureManager = new FigureManager(db);
        }

        public ApplicationUserManager UserManager { get; }

        public ApplicationRoleManager RoleManager { get; }

        public IPlayerManager PlayerManager { get; }

        public IGameManager GameManager { get; }

        public IFigureManager FigureManager { get; }

        public ITableManager TableManager { get; }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose managers managed resorces.
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    UserManager.Dispose();
                    TableManager.Dispose();
                    GameManager.Dispose();
                    RoleManager.Dispose();
                    PlayerManager.Dispose();
                }
                this.disposed = true;
            }
        }


    }
}
