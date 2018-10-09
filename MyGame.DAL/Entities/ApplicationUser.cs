using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyGame.DAL.Identity;

namespace MyGame.DAL.Entities
{
    /// <summary>
    /// Custom user class with <see cref="Int32"/> as key, <see cref="UserLogin"/> as login,
    /// <see cref="UserRole"/> as users role, <see cref="UserClaim"/> as users claim.
    /// </summary>
    public class ApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {

        /// <summary>
        /// Asynchronous creating claims identity.
        /// </summary>
        /// <param name="manager">User manager.</param>
        /// <returns>Returns created claims identity.</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        /// <summary>
        /// Contains link on appropriate player profile.
        /// </summary>
        public virtual PlayerProfile PlayerProfile { get; set; }

        public virtual ICollection<Table> Tables { get; set; }

        public ApplicationUser()
        {
            Tables = new HashSet<Table>();
        }
    }
}
