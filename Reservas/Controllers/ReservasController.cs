using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Reservas.Models;

namespace Reservas.Controllers
{
    public class ReservasController : Controller
    {
        private ReservaContext db = new ReservaContext();

        // GET: Reservas
        public ActionResult Index()
        {
            return View(db.Reservas.ToList());
        }

        // GET: Reservas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // GET: Reservas/Create
        /*public ActionResult Create()
        {
            return View();
        }*/

        // POST: Reservas/Create
        public ActionResult Create(Reserva reserva)
        {
            if (reserva != null)
            {
                
                // Imprimir las variables recibidas desde el frontend
                Debug.WriteLine($"nombreUsuario: {reserva.nombreUsuario}");
                Debug.WriteLine($"fecha: {reserva.fecha}");
                Debug.WriteLine($"horaInicio: {reserva.horaInicio}");
                Debug.WriteLine($"horaCierre: {reserva.horaFin}");
                Debug.WriteLine($"nombreSala: {reserva.nombreSala}");
                Debug.WriteLine($"IdSala: {reserva.Idsala}");
                
                // Llamada al SP
                int resultado = db.Database.SqlQuery<int>(
                    "EXEC SP_INSERTAR_RESERVA @nombreUsuario, @fecha ,@horaInicio, @horaFin, @nombreSala, @Idsala",
                new SqlParameter("@nombreUsuario", reserva.nombreUsuario),
                new SqlParameter("@fecha", reserva.fecha),
                new SqlParameter("@horaInicio", reserva.horaInicio),
                new SqlParameter("@horaFin", reserva.horaFin),
                new SqlParameter("@nombreSala", reserva.nombreSala),
                new SqlParameter("@idSala", reserva.Idsala) // FK
                ).FirstOrDefault();

                // Si el resultado es mayor que 0, se asume que la reserva se creó correctamente
                if (resultado > 0)
                {
                    return Json(new { success = true, message = "Reserva creada exitosamente.", reservaId = resultado });
                }

                return Json(new { success = false, message = "No se pudo crear la reserva. Revisa los datos enviados." });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Los datos de la reserva no son válidos.", errors = errors });
        }



        // GET: Reservas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }


        public ActionResult Edit([Bind(Include = "Idr,nombreUsuario,fecha,horaInicio,horaFin,nombreSala")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reserva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserva reserva = db.Reservas.Find(id);
            db.Reservas.Remove(reserva);
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

    }

}
