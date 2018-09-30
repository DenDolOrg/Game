using MyGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyGame.Controllers
{
    public class AutorisationController : Controller
    {
        // GET: Autorisation
        public ActionResult Index()
        {
            return View();
        }

        // GET: Autorisation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Autorisation/Create
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        // POST: Autorisation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(Player newPlayer)
        {

                return View();
        }

        // GET: Autorisation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Autorisation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Autorisation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Autorisation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
