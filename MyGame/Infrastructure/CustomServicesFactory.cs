using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using MyGame.BLL.Interfaces;

namespace MyGame.Infrastructure
{
    public class CustomServicesFactory : ServiceFactory
    {
        private readonly IUserService _userService;
        private readonly ITableService _tableService;
        private readonly IAuthenticationManager _authenticationManager;

        /// <summary>
        /// Initialises new instance of <see cref="CustomServicesFactory"/>.
        /// </summary>
        /// <param name="userService">Custom version of <see cref="IUserService"/>.</param>
        /// <param name="tableService">Custom version of <see cref="ITableService"/>.</param>
        public CustomServicesFactory(IUserService userService, ITableService tableService, IAuthenticationManager authenticationManager)
        {
            _userService = userService;
            _tableService = tableService;
            _authenticationManager = authenticationManager;
        }

        public override IAuthenticationManager CreateAuthenticationManager()
        {
            return _authenticationManager;
        }

        public override ITableService CreateTableService()
        {
            return _tableService;
        }

        public override IUserService CreateUserService()
        {
            return _userService;
        }
    }
}