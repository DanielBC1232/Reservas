namespace Reservas.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System.Data.Entity.Migrations;
    using Reservas.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Reservas.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Reservas.Models.ApplicationDbContext context)
        {

            // Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Crear los roles si no existen (usuario normal y administrador)
            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));

            if (!roleManager.RoleExists("User"))
                roleManager.Create(new IdentityRole("User"));

            // crear un usuario admin por defecto
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var defaultAdmin = userManager.FindByEmail("admin@gmail..com");

            if (defaultAdmin == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com"
                };

                var result = userManager.Create(adminUser, "Adminpass55*");
                if (result.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }
            }
        }
    }
}
