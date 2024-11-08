using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Reservas.Models
{
    public class Reserva
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idreserva { get; set; }

        [Required]
        [StringLength(50)]
        public string nombreUsuario { get; set; }

        [Required]
        public DateTime fecha { get; set; }

        [Required]
        public DateTime horaInicio { get; set; }

        [Required]
        public DateTime horaFin { get; set; }

        [Required]
        [StringLength(50)]
        public string nombreSala { get; set; }

        //FK

        public int Idsala { get; set; }


        [ForeignKey("Idsala")]
        public Sala Sala { get; set; }
    }
}