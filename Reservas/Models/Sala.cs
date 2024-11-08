using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reservas.Models
{
    public class Sala
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idsala { get; set; }

        [Required]
        [StringLength(50)]
        public string nombreSala { get; set; }

        [Required]
        public int capacidad { get; set; }

        [Required]
        [StringLength(150)]
        public string ubicacion {  get; set; }

        [Required]
        [StringLength(200)]
        public string disponibilidadEquipo { get; set; } //herramientas, ejemplo proyector pizarra, etc

        //rango de disponibilidad

        [Required]
        public TimeSpan horaApertura { get; set; }

        [Required]
        public TimeSpan horaCierre { get; set; }

    }
}