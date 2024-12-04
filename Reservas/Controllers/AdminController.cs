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
        public ActionResult Dashboard()
        {

            return View("Dashboard");
        }


        // GET: Admin/GetData - general
        public ActionResult GetData()
        {

            var datos = db.SalaReservas.ToList();


            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/GetData
        public ActionResult GetSalaFecha()
        {
            //--listar nombres salas y fecha reserva 
            var datos = db.SalaReservas
              .Select(sr => new
              {
                  sr.SalaNombre,
                  sr.ReservaFecha
              })
              .ToList();

            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/GetData - fecha maxima
        public ActionResult GetFechaMax()
        {

            var datos = db.SalaReservas.Max(sr => sr.ReservaFecha);


            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/GetData - fecha minima
        public ActionResult GetFechaMin()
        {

            var datos = db.SalaReservas.Min(sr => sr.ReservaFecha);


            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/GetData --Sala y count
        public ActionResult GetSalaCount()
        {
            //(sala y fecha)
            var datos = db.SalaReservas
                .GroupBy(sr => sr.SalaNombre)
                .Select(g => new
                {
                    SalaNombre = g.Key,
                    TotalReservas = g.Count()
                })
                .ToList();

            return Json(datos, JsonRequestBehavior.AllowGet);
        }









    }
}
