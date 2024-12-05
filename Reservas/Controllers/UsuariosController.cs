using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Reservas.Models;

namespace Reservas.Controllers
{
    public class UsuariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Usuarios
        [Authorize(Roles = "Admin")]
        public ActionResult GetUsuarios()
        {
            //var usuarios = db.Usuarios.Where(u => u.UserName != "Admin").ToList();
            var usuarios = db.Users.ToList();


            return Json(usuarios, JsonRequestBehavior.AllowGet);
        }



    }
}
