using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyGame.Models.Abstract;

namespace MyGame.Models.Concrete
{
    public class EFPlayerRepository : IPlayerRepository
    {
        private GameDBContext context = new GameDBContext();
        public IEnumerable<Player> PlayerList
        {
            get
            {
                return context.Players;
            }
        }

        public void AddPlayer(Player player)
        {
            context.Players.Add(player);
            context.SaveChanges();
        }

        public void RemovePlayer(int id)
        {
            Player playerToDelete = context.Players.Find(id);
            if(playerToDelete != null)
            {
                context.Logins.Remove(playerToDelete.Login);
                context.Players.Remove(playerToDelete);
               
                context.SaveChanges();
            }
            
        }

        public bool TryGetPlayer(int id, out Player player)
        {
            player = context.Players.Find(id);
            if (player != null)
                return true;
            return false;
        }
    }
}