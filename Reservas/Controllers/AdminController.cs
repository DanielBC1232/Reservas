using Reservas.Models;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace Reservas.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Dashboard
        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard()
        {

            return View("Dashboard");
        }


        // GET: Admin/GetData - general
        [Authorize(Roles = "Admin")]
        public ActionResult GetData()
        {

            var datos = db.SalaReservas
              .Where(r => r.ReservaFecha != null &&
              r.ReservaHoraInicio != null &&
              r.ReservaHoraFin != null
              ).ToList();


            return Json(datos, JsonRequestBehavior.AllowGet);
        }


    }
}
