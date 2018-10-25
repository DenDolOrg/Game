using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using MyGame.BLL.Interfaces;

namespace MyGame.Infrastructure
{
    public class HttpContextServicesFactory : ServiceFactory
    {
        private Func<IUserService> UserFunc;
        private Func<IGameService> GameFunc;
        private Func<IAuthenticationManager> AuthFunc;

        /// <summary>
        /// Initialises new instance of <see cref="CustomServicesFactory"/>.
        /// </summary>
        /// <param name="userFunc">Delegate with returns realisation of <see cref="IUserService"/>.</param>
        /// <param name="tableFunc">Delegate with returns realisation of <see cref="IGameService"/>.</param>
        public HttpContextServicesFactory(Func<IUserService> userFunc, Func<IGameService> gameFunc,
            Func<IAuthenticationManager> authFunc)
        {
            UserFunc = userFunc;
            AuthFunc = authFunc;
            GameFunc = gameFunc;
        }

        public override IAuthenticationManager CreateAuthenticationManager()
        {
            return AuthFunc.Invoke();
        }

        public override IGameService CreateGameService()
        {
            return GameFunc.Invoke();
        }
        public override IUserService CreateUserService()
        {
            return UserFunc.Invoke();
        }
    }
}