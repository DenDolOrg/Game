using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using MyGame.BLL.Services;
using MyGame.BLL.Interfaces;
using MyGame.Infrastructure;
using System.Web.Mvc;
using Ninject;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using MyGame.BLL.DTO;
using Microsoft.AspNet.Identity.Owin;
using MyGame.DAL.Identity;
using MyGame.DAL.Entities;

[assembly: OwinStartup(typeof(MyGame.App_Start.Startup))]
namespace MyGame.App_Start
{
    /// <summary>
    /// Class for automatic Owin startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration method for setup authentication settings and connect UI with BLL.
        /// </summary>
        /// <param name="app">Default appication builder instance of <see cref="IAppBuilder"/></param>
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>
                    (
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: (id) => (Int32.Parse(id.GetUserId()))
                     )
                }
            });

            CreateDefaultRoles(CreateUserService()).Wait();
        }
        /// <summary>
        /// Method for creating ne instance of <see cref="IUserService"/>
        /// </summary>
        /// <returns>Returns new instance of <see cref="IUserService"/></returns>
        private IUserService CreateUserService()
        {
            NinjectControllerFactory currentFactory = (NinjectControllerFactory)ControllerBuilder.Current.GetControllerFactory();
            IUserService service = currentFactory.GetCurrentKernel().Get<IUserService>();
            return service;
        }
        /// <summary>
        /// Method for adding standard roles to DB and setting up user with email "dolichdenis@gmail.com" as administator.
        /// </summary>
        /// <param name="service">Instance of <see cref="IUserService"/> for BLL connection.</param>
        private async Task CreateDefaultRoles(IUserService service)
        {
            await service.SetInitialData(new UserDTO
            {
                Email = "dolichdenis@gmail.com"
            }, new List<string> { "admin", "user"});
        }
    }
}