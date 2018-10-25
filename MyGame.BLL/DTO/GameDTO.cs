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
        /// Table figures data.
        /// </summary>
        public ICollection<UserDTO> Opponents { get; set; }

    }
}
