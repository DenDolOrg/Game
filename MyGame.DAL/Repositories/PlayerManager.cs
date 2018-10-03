using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using MyGame.DAL.EntityFramework;
using System.Threading.Tasks;

namespace MyGame.DAL.Repositories
{
    /// <summary>
    /// Realisation of interface <see cref="IPlayerManager"/>. Class for profile control.
    /// </summary>
    public class PlayerManager : IPlayerManager
    {

        /// <summary>
        /// Database context.
        /// </summary>
        public ApplicationContext Database { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="PlayerManager"/>.
        /// </summary>
        /// <param name="db">Database context to receive.</param>
        public PlayerManager(ApplicationContext db)
        {
            Database = db;
        }

        /// <summary>
        /// Creates new player profile and adds it to DB.
        /// </summary>
        /// <param name="profile">Player profile to add.</param>
        public async Task CreateAsync(PlayerProfile profile)
        {
            Database.PlayerProfile.Add(profile);
            await Database.SaveChangesAsync();
        }

        /// <summary>
        /// Dispose Database managed resorces.
        /// </summary>
        public void Dispose()
        {
            Database.Dispose();
        }
        /// <summary>
        /// Delete users profile.
        /// </summary>
        /// <param name="profile">Profile to delete.</param>
        public async Task DeleteAsync(PlayerProfile profile)
        {
            Database.PlayerProfile.Remove(profile);
            await Database.SaveChangesAsync();
        }
    }
}
