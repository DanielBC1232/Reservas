using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Reservas.Startup))]
namespace Reservas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
