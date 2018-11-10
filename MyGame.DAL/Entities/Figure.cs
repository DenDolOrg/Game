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
    /// Game figure class.
    /// </summary>
    public class Figure
    {
        /// <summary>
        /// Id of figure.
        /// </summary>
        [Key]
        public int Id { get; set; }

        [ForeignKey("Table")]
        public int TableId { get; set; }

        /// <summary>
        /// Figure color.
        /// </summary>
        public Colors Color { get; set; }

        /// <summary>
        /// X coordinate of figure.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate of figure.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Table of current fugure.
        /// </summary>
        public virtual Table Table { get; set; }
    }
}
