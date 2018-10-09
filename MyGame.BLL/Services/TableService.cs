using MyGame.BLL.DTO;
using MyGame.BLL.Infrastructure;
using MyGame.BLL.Interfaces;
using MyGame.DAL.Entities;
using MyGame.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.BLL.Services
{
    public class TableService: ITableService
    {

        /// <summary>
        /// Instance of <see cref="IUnitOfWork"/> which contains managers of entities and DB context;
        /// </summary>
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="UserService"/>.
        /// </summary>
        /// <param name="db">Instance of <see cref="IUnitOfWork"/></param>
        public TableService(IUnitOfWork db)
        {
            Database = db;
        }

        #region CREATE
        public void CreateNewTable(UserDTO firstPlayer)
        {
            ApplicationUser user = Database.UserManager.FindByNameAsync(firstPlayer.UserName).Result;
            if (user != null)
            {
                Table newTable = new Table()
                {
                    Opponents = new List<ApplicationUser>
                    {
                        user
                    }
                };
                Database.TableManager.Create(newTable);

                Database.FigureManager.Create(newTable.Id);

                user.Tables.Add(newTable);

                Database.SaveAsync();
            }
        }
        #endregion

        #region DELETE
        public void DeteteTable(int tableId)
        {
            Table table = Database.TableManager.FindById(tableId);
            if (table != null)
            {
                Database.FigureManager.Delete(table.Id);
                Database.TableManager.Delete(table);
            }

        }
        #endregion

        #region GET_FIGURES
        public IEnumerable<FigureDTO> GetFiguresOnTable(int tableId)
        {

            IEnumerable<Figure> tableFigures = Database.FigureManager.GetFiguresForTable(tableId);
            if (tableFigures != null)
            {
                List<FigureDTO> figureDTOs = new List<FigureDTO>();
                
                foreach(Figure f in tableFigures)
                {
                    figureDTOs.Add(new FigureDTO
                    {
                        Id = f.Id.ToString(),
                        XCoord = f.X.ToString(),
                        YCoord = f.Y.ToString(),
                        Color = f.Color.ToString()
                    });
                }
                return figureDTOs.AsEnumerable();
            }
            return null;
        }
        #endregion

        #region GET_TABLE
        public TableDTO GetTable(int tableId)
        {
            Table table = Database.TableManager.FindById(tableId);
            if(table != null)
            {
                TableDTO tableDTO = new TableDTO
                {
                    Id = table.Id.ToString(),
                    Opponents = GetOpponents(table)
            };

                return tableDTO;
            }
            return null;
        }
        #endregion

        #region GET_ALL_TABLES
        public IEnumerable<TableDTO> GetAllTables()
        {
            IEnumerable<Table> tables = Database.TableManager.GetAllTabes();

            List<TableDTO> tableDTOs = new List<TableDTO>();

            IEnumerable<UserDTO> opponents;
            for (int i = 0; i < tables.Count(); i++)
            {
                Table t = tables.ElementAt(i);
                opponents = GetOpponents(t);
                tableDTOs.Add(new TableDTO
                {
                    Id = t.Id.ToString(),
                    Opponents = opponents
                });
            }
            return tableDTOs;
        }
        #endregion

        #region GET_ALL_TABLES_FOR_USER
        public ICollection<TableDTO> GetTablesForUser(UserDTO userDTO)
        {
            ApplicationUser user = Database.UserManager.FindByNameAsync(userDTO.UserName).Result;

            if (user == null)
                return null;
            ICollection<Table> tables = Database.TableManager.GetTablesForUser(user.Id).ToList();

            List<TableDTO> tableDTOs = new List<TableDTO>();

            ICollection<UserDTO> opponents;
            for (int i = 0; i < tables.Count(); i++)
            {
                Table t = tables.ElementAt(i);
                opponents = GetOpponents(t);
                tableDTOs.Add(new TableDTO
                {
                    Id = t.Id.ToString(),
                    Opponents = opponents
                });
            }
            return tableDTOs;
        }
        #endregion
        #region HELPERS

        /// <summary>
        /// Returns list of opponents on Table.
        /// </summary>
        /// <param name="table">Table to scan.</param>
        /// <returns>List of universal table data model. Each of it contains Id of table and list of opponents.</returns>
        private ICollection<UserDTO> GetOpponents(Table table)
        {
            List<UserDTO> opponents = new List<UserDTO>
            {
                new UserDTO(),
                new UserDTO()
            };
            int i = 0;
            foreach (ApplicationUser u in table.Opponents)
            {
                opponents[i++] = new UserDTO
                {
                    Id = u.Id.ToString(),
                    Name = u.PlayerProfile.Name,
                    Surname = u.PlayerProfile.Surname,
                    Email = u.Email,
                    UserName = u.UserName
                };
            }

            return opponents;
        }
        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }


    }
}
