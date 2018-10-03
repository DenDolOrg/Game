using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyGame.DAL.Entities;
namespace MyGame.DAL.Identity
{
    /// <summary>
    /// Custom role manager with <see cref="Int32"/> key.
    /// </summary>
    public class ApplicationRoleManager : RoleManager<ApplicationRole, int>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationRoleManager"/>.
        /// </summary>
        /// <param name="store">Custom role store, <see cref="RoleStore{ApplicationRole, int, UserRole}"/></param>
        /// <seealso cref="ApplicationRole"/>
        /// <seealso cref="UserRole"/>
        public ApplicationRoleManager(RoleStore<ApplicationRole, int, UserRole> store) : base(store)
        {
        }
    }
}
