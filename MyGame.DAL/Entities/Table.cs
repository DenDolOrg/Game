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
        public int Id { get; set; }

        /// <summary>
        /// Creation time of table.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// List of figures on table.
        /// </summary>
        public ICollection<Figure> Figures { get; set; }

        /// <summary>
        /// List of opponents playing on this table.
        /// </summary>
        public virtual ICollection<ApplicationUser> Opponents { get; set; }


        public Table()
        {
            CreationTime = DateTime.Now;
            Opponents = new HashSet<ApplicationUser>();
        }
    }
}
