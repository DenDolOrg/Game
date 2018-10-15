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
        private Func<ITableService> TableFunc;
        private Func<IAuthenticationManager> AuthFunc;

        /// <summary>
        /// Initialises new instance of <see cref="CustomServicesFactory"/>.
        /// </summary>
        /// <param name="userFunc">Delegate with returns realisation of <see cref="IUserService"/>.</param>
        /// <param name="tableFunc">Delegate with returns realisation of <see cref="ITableService"/>.</param>
        public HttpContextServicesFactory(Func<IUserService> userFunc, Func<ITableService> tableFunc, Func<IAuthenticationManager> authFunc)
        {
            UserFunc = userFunc;
            TableFunc = tableFunc;
            AuthFunc = authFunc;
        }

        public override IAuthenticationManager CreateAuthenticationManager()
        {
            return AuthFunc.Invoke();
        }

        public override ITableService CreateTableService()
        {
            return TableFunc.Invoke();
        }

        public override IUserService CreateUserService()
        {
            return UserFunc.Invoke();
        }
    }
}