using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyGame.Models
{
    /// <summary>
    /// Container class for saving user's registration information.
    /// </summary>
    public class RegisterModel
    {

        /// <summary>
        /// User's name.
        /// </summary>
        [Required(ErrorMessage = "Name required.")]
        public string Name { get; set; }

        /// <summary>
        /// User's surname.
        /// </summary>
        [Required(ErrorMessage = "Surname required.")]
        public string Surname { get; set; }
            
        /// <summary>
        /// User's nickname.
        /// </summary>
        [Required(ErrorMessage = "Nickname required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Nickname should be at least 3 symbols and less then 30 symbols length.")]
        [RegularExpression(@"^[a-zA-Z]\w*", ErrorMessage = "Nickname has to begin from a letter.")]
        public string Nickname { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        [Required(ErrorMessage = "Email required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        [Required(ErrorMessage = "Password required.")]
        [MinLength(6, ErrorMessage = "Passwords must be at least 6 characters.")]
        [DataType(DataType.Password, ErrorMessage = "Enter password.")]
        public string Password { get; set; }

        /// <summary>
        /// Field for password confirmation.
        /// </summary>
        [Required(ErrorMessage = "Confirm your password.")]
        [DataType(DataType.Password, ErrorMessage = "Enter password.")]
        [Compare("Password", ErrorMessage = "Not the same password")]
        public string ConfirmPassword { get; set; }

    }
}