using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.DAL.EntityFramework;
using System.Threading.Tasks;

namespace MyGame.DAL.Repositories
{
    /// <summary>
    /// Realisation of interface <see cref="IManager"/>. Class for profile control.
    /// </summary>
    public class PlayerManager : IPlayerManager
    {

        /// <summary>
        /// Database context.
        /// </summary>
        public ApplicationContext Database { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="PlayerManager"/>.
        /// </summary>
        /// <param name="db">Database context to receive.</param>
        public PlayerManager(ApplicationContext db)
        {
            Database = db;
        }

        public async Task<bool> CreateAsync(PlayerProfile profile)
        {
            try
            {
                Database.PlayerProfile.Add(profile);
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task<bool> DeleteAsync(PlayerProfile item)
        {
            try
            {
                Database.PlayerProfile.Remove(item);
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;

 
        }
    }
}
