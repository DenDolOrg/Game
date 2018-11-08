using MyGame.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGame.Models
{
    /// <summary>
    /// Class which contains main properties of game.
    /// </summary>
    public class GameModel
    {
        /// <summary>
        /// List of figures on the table.
        /// </summary>
        public IEnumerable<FigureDTO> Figures { get; set; }

        /// <summary>
        /// Id of current game.
        /// </summary>
        public int GameId { get; set; }
        
        /// <summary>
        /// Id of current player. 
        /// </summary>
        public int ThisPlayerId { get; set; }       
        
        /// <summary>
        /// Username of the opponent.
        /// </summary>
        public string OpponentName { get; set; }

        /// <summary>
        /// Username of current user.
        /// </summary>
        public string MyName { get; set; }

        /// <summary>
        /// True if now is current player's turn.
        /// </summary>
        public bool isMyTurn { get; set; }

        /// <summary>
        /// Id of player who plays with white figures.
        /// </summary>
        public int WhiteId { get; set; }

        /// <summary>
        /// Id of player who plays with black figures.
        /// </summary>
        public int BlackId { get; set; }
    }
}