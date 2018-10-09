using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGame.Models
{
    /// <summary>
    /// Model of Player.
    /// </summary>
    public class PlayerModel
    {

        /// <summary>
        /// Player's Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Full name of player.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Player's nickname.
        /// </summary>
        public string Nickname { get; set; }
    }
}