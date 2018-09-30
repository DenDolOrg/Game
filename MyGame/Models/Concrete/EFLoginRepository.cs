using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyGame.Models.Abstract;

namespace MyGame.Models.Concrete
{
    public class EFLoginRepository : ILoginRepository
    {
        private GameDBContext context = new GameDBContext();
        public IEnumerable<Login> LoginList
        {
            get
            {
                return context.Logins;
            }
        }
        public bool TryGetLogin(int id, out Login login)
        {
            login = context.Logins.Find(id);
            if (login != null)
                return true;
            return false;
        }
    }
}