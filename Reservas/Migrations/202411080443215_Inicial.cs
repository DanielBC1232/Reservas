namespace Reservas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Reservas");
            DropPrimaryKey("dbo.Usuarios");
            CreateTable(
                "dbo.Salas",
                c => new
                    {
                        Idsala = c.Int(nullable: false, identity: true),
                        nombreSala = c.String(nullable: false, maxLength: 50),
                        capacidad = c.Int(nullable: false),
                        ubicacion = c.String(nullable: false, maxLength: 150),
                        disponibilidadEquipo = c.String(nullable: false, maxLength: 200),
                        horaApertura = c.Time(nullable: false, precision: 7),
                        horaCierre = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Idsala);
            
            AddColumn("dbo.Reservas", "Idreserva", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Reservas", "Idsala", c => c.Int(nullable: false));
            AddColumn("dbo.Usuarios", "Idusuario", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Reservas", "Idreserva");
            AddPrimaryKey("dbo.Usuarios", "Idusuario");
            CreateIndex("dbo.Reservas", "Idsala");
            AddForeignKey("dbo.Reservas", "Idsala", "dbo.Salas", "Idsala", cascadeDelete: true);
            DropColumn("dbo.Reservas", "Idr");
            DropColumn("dbo.Usuarios", "Idu");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "Idu", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Reservas", "Idr", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Reservas", "Idsala", "dbo.Salas");
            DropIndex("dbo.Reservas", new[] { "Idsala" });
            DropPrimaryKey("dbo.Usuarios");
            DropPrimaryKey("dbo.Reservas");
            DropColumn("dbo.Usuarios", "Idusuario");
            DropColumn("dbo.Reservas", "Idsala");
            DropColumn("dbo.Reservas", "Idreserva");
            DropTable("dbo.Salas");
            AddPrimaryKey("dbo.Usuarios", "Idu");
            AddPrimaryKey("dbo.Reservas", "Idr");
        }
    }
}
