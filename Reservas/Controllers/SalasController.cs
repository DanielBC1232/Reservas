using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Reservas.Models;

namespace Reservas.Controllers
{
    public class SalasController : Controller
    {
        private ReservaContext db = new ReservaContext();

        // GET: Salas
        public ActionResult Index()
        {
            return View(db.Salas.ToList());
        }

        public ActionResult GetSala()
        {
            var salas = db.Salas.ToList();
            return Json(salas, JsonRequestBehavior.AllowGet);
        }

        // GET: Salas/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Salas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public JsonResult VerificarReserva(string nombreSala, string horaInicio, string horaCierre, string fecha)
        {
            // Convertir horaInicio y horaCierre a TimeSpan
            TimeSpan horaInicioTS = TimeSpan.Parse(horaInicio);
            TimeSpan horaCierreTS = TimeSpan.Parse(horaCierre);

            //DateTime fechaReserva = DateTime.Parse(fecha);

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
