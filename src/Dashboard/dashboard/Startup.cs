using Microsoft.Owin;
using Owin;
using System.Web.Hosting;

[assembly: OwinStartup(typeof(dashboard.Startup))]

namespace dashboard
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //HostingEnvironment.RegisterObject(new PingTimer());
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }
    }
}
