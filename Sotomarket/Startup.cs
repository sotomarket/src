using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sotomarket.Startup))]
namespace Sotomarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
