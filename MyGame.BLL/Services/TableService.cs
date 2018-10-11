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
        public async Task CreateNewTable(UserDTO firstPlayer)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(firstPlayer.UserName);
            if (user != null)
            {
                Table newTable = new Table()
                {
                    Opponents = new List<ApplicationUser>
                    {
                        user
                    }
                };
                await Database.TableManager.CreateAsync(newTable);

                await Database.FigureManager.CreateAsync(newTable.Id);

                user.Tables.Add(newTable);

                await Database.SaveChangesAsync();
            }
        }
        #endregion

        #region DELETE
        public async Task DeteteTable(int tableId)
        {
            Table table = Database.TableManager.FindById(tableId);
            if (table != null)
            {
                await Database.FigureManager.DeleteAsync(table.Id);
                await Database.TableManager.DeleteAsync(table);                
            }
        }
        #endregion

        #region GET_FIGURES
        public async Task<IEnumerable<FigureDTO>> GetFiguresOnTable(int tableId)
        {
            IEnumerable<Figure> tableFigures = await Database.FigureManager.GetFiguresForTable(tableId).ToListAsync();
            if (tableFigures != null)
            {
                return CreateFiguresDTO(tableFigures);
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
        public async Task<IEnumerable<TableDTO>> GetAllTables()
        {
            IEnumerable<Table> tableList =  await Database.TableManager.GetAllTabes().ToListAsync();

            return CreateTablesDTO(tableList);
        }
        #endregion

        #region GET_TABLES_FOR_USER
        public async Task<IEnumerable<TableDTO>> GetUserTables(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userDTO.UserName);

            if (user == null)
                return null;
            IEnumerable<Table> tables = await Database.TableManager.GetTablesForUser(user.Id).ToListAsync();

            return CreateTablesDTO(tables);
        }
        #endregion

        #region AVAILABLE_TABLES
        public async Task<IEnumerable<TableDTO>> GetAvailableTables(UserDTO userDTO)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userDTO.UserName);
            if (user == null)
                return null;

            IEnumerable<Table> tables = await Database.TableManager.GetAvailableTables(user.Id).ToListAsync();

            return CreateTablesDTO(tables);
        }
        #endregion

        #region HELPERS

        /// <summary>
        /// Returns list of opponents on Table.
        /// </summary>
        /// <param name="table">Table to scan.</param>
        /// <returns>List of universal table data model. Each of it contains Id of table and list of opponents.</returns>
        private IEnumerable<UserDTO> GetOpponents(Table table)
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

        /// <summary>
        /// Creates list of <see cref="TableDTO"/> for list of <see cref="Table"/>
        /// </summary>
        /// <param name="tables">Table list.</param>
        /// <returns>List of tableDTOs.</returns>
        private IEnumerable<TableDTO> CreateTablesDTO(IEnumerable<Table> tables)
        {
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

        private IEnumerable<FigureDTO> CreateFiguresDTO(IEnumerable<Figure> figures)
        {
            List<FigureDTO> figureDTOs = new List<FigureDTO>();

            foreach (Figure f in figures)
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
        #endregion

        public void Dispose()
        {
            Database.Dispose();
        }


    }
}
