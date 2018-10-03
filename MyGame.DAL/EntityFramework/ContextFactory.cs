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
            ConnectionString = "Data Source=DESKTOP-F2L76L7;Initial Catalog=GameDB;Integrated Security=True";
        }
        public ApplicationContext Create()
        {
            return new ApplicationContext(ConnectionString);
        }
    }
}
