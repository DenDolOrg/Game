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

namespace MyGame.Controllers
{
    [Authorize]
    public class TableController : Controller
    {
        #region SERVICES
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private ITableService TableService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ITableService>();
            }
        }

        /// <summary>
        /// Returns new instance of <see cref="IAuthenticationManager"/> for managing authentication process.
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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
            await TableService.DeteteTable(id);

        }
        #endregion
    }
}
