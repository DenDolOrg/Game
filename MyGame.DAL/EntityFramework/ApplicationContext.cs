using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using MyGame.DAL.Entities;
using System.Data.Entity;

namespace MyGame.DAL.EntityFramework
{
    public class ApplicationContext :IdentityDbContext<ApplicationUser,ApplicationRole, int, UserLogin, UserRole, UserClaim>
    {
        public ApplicationContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<PlayerProfile> PlayerProfile { get; set; }
        public DbSet<Table> Tables { get; set; }

        public DbSet<Figure> Figures { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<PlayerProfile>().ToTable("Profile");
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
        }

        //public System.Data.Entity.DbSet<MyGame.Models.TableModel> TableModels { get; set; }

        //public static ApplicationContext Create()
        //{
        //    return new ApplicationContext();
        //}
    }
}
