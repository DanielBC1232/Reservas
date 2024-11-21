namespace Reservas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReserva : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservas", "horaInicio", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Reservas", "horaFin", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reservas", "horaFin", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Reservas", "horaInicio", c => c.DateTime(nullable: false));
        }
    }
}
