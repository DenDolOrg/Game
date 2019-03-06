using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.EntityFramework
{
    public class ContextFactory : IDbContextFactory<ApplicationContext>
    {
        public string ConnectionString { get; set; }
        public ContextFactory()
        {
            ConnectionString = "Data Source=SQL6006.site4now.net;Initial Catalog=DB_A4631E_GameDB;User Id=DB_A4631E_GameDB_admin;Password=10dfhtybrsd;";
        }
        public ApplicationContext Create()
        {
            return new ApplicationContext(ConnectionString);
        }
    }
}
