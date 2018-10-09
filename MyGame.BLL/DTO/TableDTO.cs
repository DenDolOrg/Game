﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.DTO
{
    /// <summary>
    /// Main table data model to transter. 
    /// </summary>
    public class TableDTO
    {
        /// <summary>
        /// Table Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Table figures data.
        /// </summary>
        public IEnumerable<UserDTO> Opponents { get; set; }
    }
}
