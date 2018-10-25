using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MyGame.DAL.Entities;

namespace MyGame.DAL.Identity
{
    /// <summary>
    /// Custom user manager with <see cref="Int32"/> key and <see cref="ApplicationUser"/> user.
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationUserManager"/>.
        /// </summary>
        /// <param name="store">Custom user store, <see cref="UserStore{ApplicationUser, int}"/></param>
        /// <seealso cref="ApplicationUser"/>
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store) : base(store)
        {
        }


    }
}
