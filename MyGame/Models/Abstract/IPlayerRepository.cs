using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Models.Abstract
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> PlayerList { get;}
        void AddPlayer(Player login);
        void RemovePlayer(int id);
        bool TryGetPlayer(int id, out Player Player);
    }
}
