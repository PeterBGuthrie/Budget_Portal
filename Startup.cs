using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Budget_Portal.Startup))]
namespace Budget_Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
