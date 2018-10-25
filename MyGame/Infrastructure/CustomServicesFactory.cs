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
        private readonly IGameService _gameService;
        private readonly IAuthenticationManager _authenticationManager;

        /// <summary>
        /// Initialises new instance of <see cref="CustomServicesFactory"/>.
        /// </summary>
        /// <param name="userService">Custom version of <see cref="IUserService"/>.</param>
        /// <param name="tableService">Custom version of <see cref="IGameService"/>.</param>
        public CustomServicesFactory(IUserService userService, IGameService gameService, IAuthenticationManager authenticationManager)
        {
            _userService = userService;
            _gameService = gameService;
            _authenticationManager = authenticationManager;
        }

        public override IAuthenticationManager CreateAuthenticationManager()
        {
            return _authenticationManager;
        }

        public override IGameService CreateGameService()
        {
            return _gameService;
        }

        public override IUserService CreateUserService()
        {
            return _userService;
        }
    }
}