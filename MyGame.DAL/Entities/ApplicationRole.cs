using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyGame.DAL.Entities
{
    /// <summary>
    /// Custom identity role with <see cref="Int32"/> as key, <see cref="UserRole"/> as users role.
    /// </summary>
    public class ApplicationRole : IdentityRole<int, UserRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole" /> class.
        /// </summary>
        public ApplicationRole()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole" /> class with role name parameter.
        /// </summary>
        /// <param name="name">Role name.</param>
        public ApplicationRole(string name)
        {
            Name = name;
        }
    }
}
