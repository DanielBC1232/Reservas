using Reservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reservas.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ReservaContext db = new ReservaContext();

        // GET: Accounts/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Accounts/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Idu,nombreUsuario,correo,rol,PasswordHash,PasswordSalt")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Register");
            }

            return View(usuario);
        }


        // POST: Accounts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Idu,nombreUsuario,correo,rol,PasswordHash,PasswordSalt")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(usuario);
        }

        // Metodo para verificar si el correo ya esta registrado
        [HttpPost]
        public JsonResult CheckEmail(string correo)
        {
            bool exists = db.Usuarios.Any(u => u.correo == correo);
            return Json(!exists); // Retorna true si el correo esta disponible
        }

    }
}
