using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Entities
{
    public class Game
    {
        /// <summary>
        /// Game Id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Id of player who plays with white figures.
        /// </summary>
        public int  WhitePlayerId{ get; set; }

        /// <summary>
        /// Id of player who plays with black figures.
        /// </summary>
        public int  BlackPlayerId{ get; set; }
        /// <summary>
        /// List of opponents playing on this table.
        /// </summary>
        public virtual ICollection<ApplicationUser> Opponents { get; set; }

        public virtual Table Table { get; set; }

        public Game()
        {
            Opponents = new HashSet<ApplicationUser>();
        }
    }
}
