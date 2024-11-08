using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservas.Models
{
    public class Usuario
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idusuario { get; set; }

        [Required]
        [StringLength(50)]
        public string nombreUsuario { get; set; }

        [Required]
        [StringLength(75)]
        public string correo { get; set; }

        [Required]
        [StringLength(25)]
        public string rol {  get; set; }

        [Required]  //para el hash de contrasena
        public byte[] PasswordHash { get; set; }

        [Required]  //Salt para variar el hash
        public byte[] PasswordSalt { get; set; }

    }
}