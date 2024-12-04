namespace Reservas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Identity1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VW_SALA_RESERVA",
                c => new
                    {
                        SalaNombre = c.String(nullable: false, maxLength: 128),
                        Capacidad = c.Int(nullable: false),
                        Ubicacion = c.String(),
                        DisponibilidadEquipo = c.String(),
                        HoraApertura = c.Time(nullable: false, precision: 7),
                        HoraCierre = c.Time(nullable: false, precision: 7),
                        ReservaFecha = c.DateTime(),
                        ReservaHoraInicio = c.Time(nullable: false, precision: 7),
                        ReservaHoraFin = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SalaNombre);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VW_SALA_RESERVA");
        }
    }
}
