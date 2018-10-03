using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.DTO
{
    /// <summary>
    /// Universal user data model to transter.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Users id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Users name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Users surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Users nickname.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Users email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Users password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Users role.
        /// </summary>
        public string Role { get; set; }

    }
}
