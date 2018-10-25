using Microsoft.Owin.Security;
using MyGame.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGame.Infrastructure
{
    /// <summary>
    /// Factory for creating services.
    /// </summary>
    public abstract class ServiceFactory
    {
        /// <summary>
        /// Creates new realisation of <see cref="IUserService"/>.
        /// </summary>
        /// <returns>Realization of <see cref="IUserService"/>.</returns>
        public abstract IUserService CreateUserService();

        /// <summary>
        /// Creates new realisation of <see cref="IGameService"/>.
        /// </summary>
        /// <returns>Realization of <see cref="IGameService"/>.</returns>
        public abstract IGameService CreateGameService();

        public abstract IAuthenticationManager CreateAuthenticationManager();
    }
}