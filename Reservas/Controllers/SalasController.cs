using Reservas.Models;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Reservas.Controllers
{
    public class SalasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Salas
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Salas.ToList());
        }

        [Authorize]
        public ActionResult GetSala()
        {
            var salas = db.Salas.ToList();
            return Json(salas, JsonRequestBehavior.AllowGet);
        }

        // GET: Salas/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sala sala = db.Salas.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            return View(sala);
        }

        // GET: Salas/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Salas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Idsala,nombreSala,capacidad,ubicacion,disponibilidadEquipo,horaApertura,horaCierre")] Sala sala)
        {
            if (ModelState.IsValid)
            {
                db.Salas.Add(sala);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sala);
        }

        // GET: Salas/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sala sala = db.Salas.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            return View(sala);
        }

        // POST: Salas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Idsala,nombreSala,capacidad,ubicacion,disponibilidadEquipo,horaApertura,horaCierre")] Sala sala)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sala).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sala);
        }

        // GET: Salas/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sala sala = db.Salas.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            return View(sala);
        }

        // POST: Salas/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sala sala = db.Salas.Find(id);
            db.Salas.Remove(sala);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [Authorize]
        public JsonResult VerificarReserva(string nombreSala, string horaInicio, string horaCierre, string fecha)
        {
            // Convertir horaInicio y horaCierre a TimeSpan
            TimeSpan horaInicioTS = TimeSpan.Parse(horaInicio);
            TimeSpan horaCierreTS = TimeSpan.Parse(horaCierre);

            // Llamar al SP para verificar disponibilidad
            int resultado = db.Database.SqlQuery<int>(
                "EXEC SP_VERIFICAR_RESERVA @nombreSala, @horaInicio, @horaCierre, @fecha",
                new SqlParameter("@nombreSala", nombreSala),
                new SqlParameter("@horaInicio", horaInicioTS),
                new SqlParameter("@horaCierre", horaCierreTS),
                new SqlParameter("@fecha", fecha)
            ).FirstOrDefault();

            if (resultado == 0)
            {
                return Json(new { success = true, message = "La sala está disponible." });
            }
            else
            {
                return Json(new { success = false, message = "La sala no está disponible en la fecha y horas seleccionadas." });
            }
        }

    }

}
