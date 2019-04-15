using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project3.Startup))]
namespace Project3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
