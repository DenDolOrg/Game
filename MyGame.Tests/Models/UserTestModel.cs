using MyGame.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests.Models
{
    internal class UserTestModel
    {
        internal UserDTO UserDTO { get; set; }
        internal ICollection<TableDTO> Tables { get; set; }
    }
}
