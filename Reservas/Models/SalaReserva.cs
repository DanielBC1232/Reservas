using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reservas.Models
{
    [Table("VW_SALA_RESERVA")]
    public class SalaReserva
    {
        [Key]
        [Column("SalaNombre")]
        public string SalaNombre { get; set; }

        [Column("Capacidad")]
        public int Capacidad { get; set; }

        [Column("Ubicacion")]
        public string Ubicacion { get; set; }

        [Column("DisponibilidadEquipo")]
        public string DisponibilidadEquipo { get; set; }

        [Column("HoraApertura")]
        public TimeSpan HoraApertura { get; set; }

        [Column("HoraCierre")]
        public TimeSpan HoraCierre { get; set; }

        [Column("ReservaFecha")]
        public DateTime? ReservaFecha { get; set; }

        [Column("ReservaHoraInicio")]
        public TimeSpan ReservaHoraInicio { get; set; }

        [Column("ReservaHoraFin")]
        public TimeSpan ReservaHoraFin { get; set; }
    }
}