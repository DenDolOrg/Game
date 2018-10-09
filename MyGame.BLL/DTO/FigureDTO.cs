using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.DTO
{
    /// <summary>
    /// Main figure data to transfer.
    /// </summary>
    public class FigureDTO
    {
        /// <summary>
        /// Figure Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// X coordinate of figure.
        /// </summary>
        public string XCoord { get; set; }

        /// <summary>
        /// Y coordinate of figure.
        /// </summary>
        public string YCoord { get; set; }

        /// <summary>
        /// Figure color.
        /// </summary>
        public string Color { get; set; }
    }
}
