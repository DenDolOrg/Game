using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGame.Models
{
    /// <summary>
    /// Class which contains properties to write needed for adding new game.
    /// </summary>
    public class NewGameModel
    {
        /// <summary>
        /// Color of figures for player who creates game.
        /// </summary>
        public string FirstColor { get; set; }
    }
}