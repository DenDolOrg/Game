using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGame.Models
{
    /// <summary>
    /// Class which contains properties for single square on the table.
    /// </summary>
    public class FieldModel
    {
        /// <summary>
        /// X coord of square.
        /// </summary>
        public int X { get; set; }
        
        /// <summary>
        /// Y coord of square.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Color of square.
        /// </summary>
        public string Color { get; set; }
    }
}