using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToolsForEver.Startup))]
namespace ToolsForEver
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
