using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.DTO
{
    /// <summary>
    /// Main table data model to transter. 
    /// </summary>
    public class GameDTO
    {
        /// <summary>
        /// Table Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Creation time of table.
        /// </summary>
        public string CreationTime { get; set; }

        /// <summary>
        /// Id of player who plays with white figures.
        /// </summary>
        public int WhitePlayerId { get; set; }

        /// <summary>
        /// Id of player who plays with black figures.
        /// </summary>
        public int BlackPlayerId { get; set; }

        /// <summary>
        /// Id of player who has made last turn.
        /// </summary>
        public int LastTurnPlayerId { get; set; }

        /// <summary>
        /// Opponents data.
        /// </summary>
        public ICollection<UserDTO> Opponents { get; set; }

    }
}
