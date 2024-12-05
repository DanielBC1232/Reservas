namespace Reservas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            DropTable("dbo.VW_SALA_RESERVA");
        }
    }
}
