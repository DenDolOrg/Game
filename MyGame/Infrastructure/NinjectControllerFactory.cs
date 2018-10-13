using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using System.Web.Routing;
using MyGame.BLL.Interfaces;
using MyGame.DAL.Interfaces;
using MyGame.BLL.Services;
using MyGame.DAL.Repositories;
using MyGame.App_Start;

namespace MyGame.Infrastructure
{
    /// <summary>
    /// Controller factory class for ninject DR
    /// </summary>
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectControllerFactory" /> class.
        /// </summary>
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        /// <summary>
        /// Returns controller instance for required context and type.
        /// </summary>
        /// <param name="requestContext">Context of HTTP-request</param>
        /// <param name="controllerType">Controller type</param>
        /// <returns>Controller instance </returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
                return (IController)ninjectKernel.Get(controllerType);
            else
                return null;
        }

        /// <summary>
        /// Adds bindings to ninject DR container
        /// </summary>
        private void AddBindings()
        {
            ninjectKernel.Bind<IUnitOfWork>().To<IdentityUnitOfWork>();
            ninjectKernel.Bind<IUserService>().To<UserService>()
                .When(request => (request.Target == null) || (request.Target.Name.EndsWith("Controller"))); ;

            ninjectKernel.Bind<ITableService>().To<TableService>()
                .When(request => (request.Target == null) || (request.Target.Name.EndsWith("Controller"))); ;

        }

        public IKernel GetCurrentKernel()
        {
            return ninjectKernel;
        }

    }
}