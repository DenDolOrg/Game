using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGame.Models
{
    /// <summary>
    /// Class for storing path for async http requests.
    /// </summary>
    public class GameActionModel
    {
        /// <summary>
        /// Action name to run like async http-request.
        /// </summary>
        public string ActionName { get; set; }
    }
}