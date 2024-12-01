﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;

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

        public ActionResult Reservas()
        {
            return View(db.Reservas.ToList());
        }


        public ActionResult UsuarioReservas(string Usuario)
        {
            try
            {
                string usuarioP = "Usuario02"; // tomar usuario de la sesion

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

        public ActionResult ObtenerReservas(string tipo)
        {
            DateTime hoy = DateTime.Now.Date;

            string usuarioP = "Usuario02"; // tomar usuario de la sesion
            //string usuarioP = "admin"; // admin


            List<Reserva> reservas = new List<Reserva>();

            //dependiendo del parametro enviado devuelve las reservas pasadas o futuras
            if (tipo == "pasado")
            {
                reservas = db.Reservas
                    .Where(s => s.nombreUsuario.ToLower() == usuarioP.ToLower() && DbFunctions.TruncateTime(s.fecha) < hoy).ToList();
            }
            else if (tipo == "futuro")
            {
                reservas = db.Reservas
                    .Where(s => s.nombreUsuario.ToLower() == usuarioP.ToLower() && DbFunctions.TruncateTime(s.fecha) >= hoy).ToList();
            }

            //vista parcial
            return PartialView("_ReservasTabla", reservas);
        }

        public ActionResult ObtenerReservasPorUsuario(string usuario)
        {
            DateTime hoy = DateTime.Now.Date;

            List<Reserva> reservas = new List<Reserva>();

            if (usuario == "todos")
            {
                //listar todo
                reservas = db.Reservas
                    .Where(s => DbFunctions.TruncateTime(s.fecha) >= hoy).ToList();
            }
            else if (usuario != "todos")
            {
                //buscar con usuario
                reservas = db.Reservas
                    .Where(s => s.nombreUsuario.ToLower() == usuario.ToLower() && DbFunctions.TruncateTime(s.fecha) >= hoy).ToList();


            }

            //vista parcial
            return PartialView("_ReservasTabla", reservas);
        }

        //cambiar por listado de usuarios
        public ActionResult ObtenerUsuarios()
        {

            List<dynamic> usuarios = new List<dynamic>
        {
            new {Nombre = "Usuario02"},
            new {Nombre = "Usuario1232"},
        };
            return Json(usuarios, JsonRequestBehavior.AllowGet);
            //return View(usuario);
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
                string usuario = "Usuario1232";//tomar de sesion
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


        public ActionResult EditPreload(int id)
        {

            var reserva = db.Reservas.Find(id);

            var result = new
            {
                Idreserva = reserva.Idreserva,
                nombreUsuario = reserva.nombreUsuario,
                fecha = reserva.fecha.ToString("yyyy-MM-dd"), // Aquí formateamos la fecha
                horaInicio = reserva.horaInicio.ToString(@"hh\:mm"), // O la lógica que necesites para la hora
                horaFin = reserva.horaFin.ToString(@"hh\:mm"),
                nombreSala = reserva.nombreSala,
                Idsala = reserva.Idsala
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult Edit([Bind(Include = "Idreserva,nombreUsuario,fecha,horaInicio,horaFin,nombreSala,Idsala")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(reserva).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Reserva actualizada correctamente." });
                }
                catch (Exception ex)
                {
                    //return Json(new { success = false, message = $"Error al guardar: {ex.Message}" });
                    var innerExceptionMessage = ex.InnerException;
                    return Json(new { success = false, message = $"Error al guardar: {innerExceptionMessage}" });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Datos inválidos.", errors });
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var reserva = db.Reservas.Find(id);
                if (reserva != null)
                {
                    db.Reservas.Remove(reserva);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Reserva eliminada correctamente." });
                }
                return Json(new { success = false, message = "Reserva no encontrada." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al eliminar: {ex.Message}" });
            }
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
