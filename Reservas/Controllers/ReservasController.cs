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

        public ActionResult UsuarioReservas(string Usuario)
        {
            try
            {
                string usuarioP = "Usuario03"; // tomar usuario de la sesion

                var hoy = DateTime.Now.Date;

                //bdFunctions truncateTime elimina las horas de las fechas
                var reservas = db.Reservas
                    .Where(s => s.nombreUsuario == usuarioP && DbFunctions.TruncateTime(s.fecha) >= hoy)
                    .ToList();
                return View(reservas); //retorna resultados filtrados
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al obtener las reservas: {ex.Message}";
                return View(new List<Reserva>()); //lista Vacia
            }
        }

        public ActionResult AdminReservas(string usuario)
        {
            try
            {
                var hoy = DateTime.Now.Date;
                //listar las reservas a futuro para cancelarlas
                //bdFunctions truncateTime elimina las horas de las fechas
                var reservas = db.Reservas
                    .Where(s => DbFunctions.TruncateTime(s.fecha) >= hoy).ToList();
                return View(reservas); //retorna resultados filtrados
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Ocurrió un error al obtener las reservas: {ex.Message}";
                return View(new List<Reserva>()); //lista Vacia
            }
        }

        public ActionResult ObtenerReservas(string param)
        {
            DateTime hoy = DateTime.Now.Date;

            string usuarioP = "Usuario02"; // tomar usuario de la sesion
            //string usuarioP = "admin"; // admin


            List<Reserva> reservas = new List<Reserva>();

            //dependiendo del parametro enviado devuelve las reservas pasadas o futuras
            if (param == "pasado")
            {
                reservas = db.Reservas
                    .Where(s => s.nombreUsuario == usuarioP && DbFunctions.TruncateTime(s.fecha) < hoy).ToList();
            }
            else if (param == "futuro")
            {
                reservas = db.Reservas
                    .Where(s => s.nombreUsuario == usuarioP && DbFunctions.TruncateTime(s.fecha) >= hoy).ToList();
            }

            //vista parcial
            return PartialView("_ReservasTabla", reservas);
        }

        public ActionResult ObtenerReservasPorUsuario(string usuario, bool tipo)
        {
            DateTime hoy = DateTime.Now.Date;

            //tipo true = todos los usuarios
            //tipo false= usuario especifico

            List<Reserva> reservas = new List<Reserva>();

            if (tipo == true)
            {
                //listar todo
                reservas = db.Reservas
                    .Where(s => DbFunctions.TruncateTime(s.fecha) >= hoy).ToList();
            }
            else
            {
                //buscar con usuario
                reservas = db.Reservas
                    .Where(s => s.nombreUsuario.StartsWith(usuario) && DbFunctions.TruncateTime(s.fecha) >= hoy).ToList();
                return View(reservas);
            }

            //vista parcial
            return PartialView("_ReservasTabla", reservas);
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


        // POST: Reservas/Create
        public ActionResult Create(Reserva reserva)
        {
            if (reserva != null)
            {
                string usuario="Usuario1232";//tomar de sesion
                // Llamada al SP
                int resultado = db.Database.SqlQuery<int>(
                    "EXEC SP_INSERTAR_RESERVA @nombreUsuario, @fecha ,@horaInicio, @horaFin, @nombreSala, @Idsala",
                new SqlParameter("@nombreUsuario", usuario),
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
