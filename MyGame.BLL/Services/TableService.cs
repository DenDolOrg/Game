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
        public async Task<OperationDetails> DeteteTable(TableDTO tableDTO)
        {
            OperationDetails successOD = new OperationDetails(true);
            OperationDetails failOD = new OperationDetails(false);

            Table table = Database.TableManager.FindById(tableDTO.Id);
            if (table == null)
                return failOD;

            bool FiguresDelResult = await Database.FigureManager.DeleteAsync(table.Id);
            bool TableDelResult = await Database.TableManager.DeleteAsync(table);

            if (!(FiguresDelResult && TableDelResult))
                return failOD;

            return successOD;
            
            
        }
        #endregion

        #region DELETE_USER_TABLES
        public async Task<OperationDetails> DeteteUserTables(UserDTO userDTO)
        {
            OperationDetails successOD = new OperationDetails(true);
            OperationDetails failOD = new OperationDetails(false);

            ApplicationUser user = await Database.UserManager.FindByIdAsync(userDTO.Id);

            if (user == null)
                return failOD;

            IEnumerable<int> tables = new List<int>(user.Tables.Select(t => t.Id));
            if (tables == null || tables.Count() == 0)
                return failOD;

            foreach(int id in tables)
            {
                var TableDelResult = await DeteteTable(new TableDTO { Id = id });
                if (!TableDelResult.Succedeed)
                    return failOD;
            }

            return successOD;
        }
        #endregion

        #region GET_FIGURES
        public async Task<IEnumerable<FigureDTO>> GetFiguresOnTable(TableDTO tableDTO)
        {
            IEnumerable<Figure> tableFigures = await Database.FigureManager.GetFiguresForTable(tableDTO.Id).ToListAsync();
            if (tableFigures != null)
            {
                return CreateFiguresDTO(tableFigures);
            }
            return null;
        }
        #endregion

        #region GET_TABLE
        public TableDTO GetTable(TableDTO tableDTO)
        {
            Table table = Database.TableManager.FindById(tableDTO.Id);
            if(table != null)
            {
                TableDTO tableDTO_out = new TableDTO
                {
                    Id = table.Id,
                    Opponents = GetOpponents(table),
                    CreationTime = table.CreationTime
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

        //#region JOIN_TO_TABLE
       
        //public async Task<OperationDetails> JoinToTable(UserDTO userDTO, TableDTO tableDTO)
        //{
        //    ApplicationUser user = await Database.UserManager.FindByNameAsync(userDTO.UserName);
        //}
        //#endregion

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
                    Id = u.Id,
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
                    Id = t.Id,
                    Opponents = opponents,
                    CreationTime = t.CreationTime
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
