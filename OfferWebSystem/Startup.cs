using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OfferWebSystem.Startup))]
namespace OfferWebSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
