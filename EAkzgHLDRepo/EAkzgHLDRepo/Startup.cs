using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EAkzgHLDRepo.Startup))]
namespace EAkzgHLDRepo
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
