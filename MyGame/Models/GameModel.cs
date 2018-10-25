using MyGame.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyGame.Models
{
    public class GameModel
    {
        public ICollection<FigureDTO> Figures { get; set; }
        public GameDTO Game { get; set; }
        public int WhiteId { get; set; }
        public int BlackId { get; set; }
    }
}