using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual int Id { get; set; }

        /// <summary>
        /// Figure color.
        /// </summary>
        public virtual Colors Color { get; set; }

        /// <summary>
        /// X coordinate of figure.
        /// </summary>
        public virtual int X { get; set; }

        /// <summary>
        /// Y coordinate of figure.
        /// </summary>
        public virtual int Y { get; set; }

        /// <summary>
        /// Table of current fugure.
        /// </summary>
        public virtual Table Table { get; set; }
    }
}
