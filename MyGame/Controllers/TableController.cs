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


        [Authorize(Roles = "admin")]
        [HttpGet]
        public JsonResult GetAllTables()
        {
            IEnumerable<object> tables = TableService.GetAllTables();

            JsonResult json = Json(tables, JsonRequestBehavior.AllowGet);
            return json;
        }
        public ActionResult AllTablesList()
        {
            ActionModel actionModel = new ActionModel { ActionAddress = "/Table/GetAllTables" };
            return View("TableList", actionModel);
        }

        public ActionResult CreateNewtable()
        {
            UserDTO user = new UserDTO { UserName = HttpContext.User.Identity.Name };
            TableService.CreateNewTable(user);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            TableService.DeteteTable(id);

            JsonResult json = Json(new { success = true });
            return json;
        }
    }
}
