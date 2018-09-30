using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyGame.Models.Abstract;

namespace MyGame.Models.Concrete
{
    /// <summary>
    /// Entity Framework Repository for Player table
    /// </summary>
    public class EFPlayerRepository : IPlayerRepository
    {
        /// <summary>
        /// New instance of Data Base context
        /// </summary>
        private GameDBContext context = new GameDBContext();
        
        /// <summary>
        /// List of <see cref="Player"/> values.
        /// </summary>
        public IEnumerable<Player> PlayerList
        {
            get
            {
                return context.Players;
            }
        }

        /// <summary>
        /// Add new <see cref="Player"/> to context and refreshes DB.
        /// </summary>
        /// <param name="player">Player to add</param>
        public void AddPlayer(Player player)
        {
            context.Players.Add(player);
            context.SaveChanges();
        }

        /// <summary>
        /// Remove <see cref="Player"/>  with some id and his <see cref="Login"/> row from context and refreshes DB.
        /// </summary>
        /// <param name="id">Player Id do remove.</param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryGetPlayer(int id, out Player player)
        {
            player = context.Players.Find(id);
            if (player != null)
                return true;
            return false;
        }
    }
}