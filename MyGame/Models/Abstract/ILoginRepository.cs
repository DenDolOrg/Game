using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyGame.Models;
namespace MyGame.Models.Abstract
{
    public interface ILoginRepository
    {
        IEnumerable<Login> LoginList { get; }
        bool TryGetLogin(int id, out Login login);
    }
}
