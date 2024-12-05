namespace Reservas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
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
                        horaInicio = c.Time(nullable: false, precision: 7),
                        horaFin = c.Time(nullable: false, precision: 7),
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
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Reservas", "Idsala", "dbo.Salas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reservas", new[] { "Idsala" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.VW_SALA_RESERVA");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Salas");
            DropTable("dbo.Reservas");
        }
    }
}
