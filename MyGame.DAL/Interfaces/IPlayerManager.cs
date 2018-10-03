using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.DAL.Entities;

namespace MyGame.DAL.Interfaces
{
    public interface IPlayerManager :IDisposable
    {
        Task CreateAsync(PlayerProfile profile);

        Task DeleteAsync(PlayerProfile profile);
    }
}
