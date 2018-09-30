using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyGame.Models.Abstract;

namespace MyGame.Models.Concrete
{
    /// <summary>
    /// Entity Framework Repository for Login table
    /// </summary>
    public class EFLoginRepository : ILoginRepository
    {
        /// <summary>
        /// New instance of Data Base context
        /// </summary>
        private GameDBContext context = new GameDBContext();

        /// <summary>
        /// List of <see cref="Login"/> values.
        /// </summary>
        public IEnumerable<Login> LoginList
        {
            get
            {
                return context.Logins;
            }
        }
        /// <summary>
        /// Tries to take Login with some id from DB context.
        /// </summary>
        /// <param name="id">Id of necessary Login</param>
        /// <param name="login">Reference of finded Login to return.</param>
        /// <returns>
        /// Returns:
        /// <list type="table">
        /// <item>
        ///     <description><c>true</c>: if the attempt was successful;</description>
        /// </item>
        /// <item>
        ///     <description><c>false</c>: if not successful.</description>
        /// </item>
        /// </list>
        /// </returns>
        public bool TryGetLogin(int id, out Login login)
        {
            login = context.Logins.Find(id);
            if (login != null)
                return true;
            return false;
        }
    }
}