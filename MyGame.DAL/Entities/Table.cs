using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Entities
{
    /// <summary>
    /// Game table class.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Id of current table.
        /// </summary>
        [Key]
        [ForeignKey("Game")]
        public int Id { get; set; }

        /// <summary>
        /// Creation time of table.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Game for this table.
        /// </summary>
        public virtual Game Game { get; set; }

        public Table()
        {
            CreationTime = DateTime.Now;
        }
    }
}
