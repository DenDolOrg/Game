using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyGame.Models
{
    /// <summary>
    /// Container class for saving user's login information. 
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// User's Email or Nickname.
        /// </summary>
        [Required(ErrorMessage = "Email required.")]
        public string Email { get; set; }
        
        /// <summary>
        ///User's password.
        /// </summary>
        [Required(ErrorMessage = "Password required.")]
        [MinLength(6, ErrorMessage = "Passwords must be at least 6 characters.")]
        [DataType(DataType.Password, ErrorMessage = "Enter password.")]
        public string Password { get; set; }
    }
}