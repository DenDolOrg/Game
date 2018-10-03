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
        [Required(ErrorMessage = "Email or Nickname required.")]
        public string EmailOrNickname { get; set; }
        
        /// <summary>
        ///User's password.
        /// </summary>
        [Required(ErrorMessage = "Password required.")]
        [DataType(DataType.Password, ErrorMessage = "Enter password.")]
        public string Password { get; set; }
    }
}