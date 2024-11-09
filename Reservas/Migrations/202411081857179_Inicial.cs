namespace Reservas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservas",
                c => new
                    {
                        Idreserva = c.Int(nullable: false, identity: true),
                        nombreUsuario = c.String(nullable: false, maxLength: 50),
                        fecha = c.DateTime(nullable: false),
                        horaInicio = c.DateTime(nullable: false),
                        horaFin = c.DateTime(nullable: false),
                        nombreSala = c.String(nullable: false, maxLength: 50),
                        Idsala = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Idreserva)
                .ForeignKey("dbo.Salas", t => t.Idsala, cascadeDelete: true)
                .Index(t => t.Idsala);
            
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
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Idusuario = c.Int(nullable: false, identity: true),
                        nombreUsuario = c.String(nullable: false, maxLength: 50),
                        correo = c.String(nullable: false, maxLength: 75),
                        rol = c.String(nullable: false, maxLength: 25),
                        PasswordHash = c.Binary(nullable: false),
                        PasswordSalt = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Idusuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservas", "Idsala", "dbo.Salas");
            DropIndex("dbo.Reservas", new[] { "Idsala" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Salas");
            DropTable("dbo.Reservas");
        }
    }
}
