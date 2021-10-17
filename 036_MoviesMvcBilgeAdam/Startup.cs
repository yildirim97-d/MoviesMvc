using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_036_MoviesMvcBilgeAdam.Startup))]
namespace _036_MoviesMvcBilgeAdam
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
