using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IntelliPackWeb.Startup))]
namespace IntelliPackWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
