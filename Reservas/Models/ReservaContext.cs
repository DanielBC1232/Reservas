using System.Data.Entity;

namespace Reservas.Models
{
    public class ReservaContext : DbContext
    {

        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<SalaReserva> SalaReservas { get; set; }


        public ReservaContext() : base("name=Reserva")
        {
        }

        public System.Data.Entity.DbSet<Reservas.Models.Sala> Salas { get; set; }

    }
}