using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyGame.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyGame.BLL.DTO;
using System.Threading.Tasks;
using MyGame.Models;
using MyGame.Infrastructure;

namespace MyGame.Controllers
{
    [Authorize]
    public class TableController : Controller
    {

        #region SERVICES
        /// <summary>
        /// Factory for creating services.
        /// </summary>
        private ServiceFactory serviceFactory;

        /// <summary>
        /// Service which contains methods to work with users.
        /// </summary>
        private IUserService UserService
        {
            get
            {
                return serviceFactory.CreateUserService();
            }
        }

        /// <summary>
        /// Service which contains methods to work with tables.
        /// </summary>
        private ITableService TableService
        {
            get
            {
                return serviceFactory.CreateTableService();
            }
        }

        /// <summary>
        /// Initialises a new instance of <see cref="TableController"/> with default services.
        /// </summary>
        public TableController()
        {
            serviceFactory = new HttpContextServicesFactory(
                () => HttpContext.GetOwinContext().Get<IUserService>(),
                () => HttpContext.GetOwinContext().Get<ITableService>(),
                () => null);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="TableController"/> with custom services(for unit testing).
        /// </summary>
        public TableController(IUserService userService, ITableService tableService, IAuthenticationManager authenticationManager)
        {
            serviceFactory = new CustomServicesFactory(userService, tableService, authenticationManager);
        }
        #endregion

        #region TABLE_LIST
        /// <summary>
        /// Decides which list to show.
        /// </summary>
        /// <param name="tableType">Table type to show.</param>
        /// <returns>View with needed list.</returns>
        [Authorize]
        public ActionResult TableList(string tableType)
        {
            TableActionModel tableAction = new TableActionModel();
            if (tableType == "all")
                tableAction.ActionName = "/Table/GetAllTables";

            else if (tableType == "available")
                tableAction.ActionName = "/Table/GetAvailableTables";

            else if (tableType == "myTables")
                tableAction.ActionName = "/Table/GetUserTables";
            else
                return null;

            return View("TableList", tableAction);
        }
        #endregion

        #region USER_TABLES
        /// <summary>
        /// Returns tables where current user is one of the opponents.
        /// </summary>
        /// <returns>Json object for table list.</returns>
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetUserTables()
        {
            UserDTO userDTO = new UserDTO { UserName = HttpContext.User.Identity.Name };

            IEnumerable<TableDTO> tables = await TableService.GetUserTables(userDTO);

            return Json(tables, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ALL_TABLES

        /// <summary>
        /// Returns all table.
        /// </summary>
        /// <returns>Json object for table list.</returns>

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<JsonResult> GetAllTables()
        {
            IEnumerable<TableDTO> tables = await TableService.GetAllTables();

            return Json(tables, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AVAILABLE_TABLES
        /// <summary>
        /// Returns available to join tables.
        /// </summary>
        /// <returns>Json object for table list.</returns>
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetAvailableTables()
        {
            UserDTO userDTO = new UserDTO { UserName = HttpContext.User.Identity.Name };

            IEnumerable<TableDTO> tables = await TableService.GetAvailableTables(userDTO);

            JsonResult json = Json(tables, JsonRequestBehavior.AllowGet);
            return json;
        }
        #endregion

        #region CREATE_TABLE
        /// <summary>
        /// Creates new table for user.
        /// </summary
        public async Task<ActionResult> CreateNewtable()
        {
            UserDTO user = new UserDTO { UserName = HttpContext.User.Identity.Name };
            await TableService.CreateNewTable(user);
            return RedirectToAction("TableList", "TAble", new { tableType = "myTables" });
        }
        #endregion

        //#region JOIN_TABLE
        //public async Task<ActionResult> JoinTable(int tableId)
        //{

        //}
        
        //#endregion

        #region DELETE_TABLE
        /// <summary>
        /// Deletes table with some id.
        /// </summary>
        /// <param name="id">Id of table to delete.</param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await TableService.DeteteTable(new TableDTO {Id = id });

        }
        #endregion
    }
}
