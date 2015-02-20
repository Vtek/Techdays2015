using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.AspNet.SignalR;

namespace dashboard
{
    public class PingTimer : IRegisteredObject
    {
        /// <summary>
        /// Timer
        /// </summary>
        private Timer Timer { get; set; }

        /// <summary>
        /// DashHub context
        /// </summary>
        private IHubContext DashHubContext { get; set; }

        public PingTimer()
        {
            DashHubContext = GlobalHost.ConnectionManager.GetHubContext<DashHub>();
 
            Start();
        }

        private void Start()
        {
            //Timer toute les 5 secondes pour ping tout les clients.
            //Le hub est obligé de faire ça pour les bolts car il y a un problème de reconnection auto dans le client SignalR Java
            //Le hack côté Java pour corriger ça est de mettre une reco cliente toutes les 5 seconde via un timer
            //Malheureusement c'est impossible car Storm empêche l'utilisation de cette classe...
            Timer = new Timer(Ping, null, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 5));
        }

        private void Ping(object state)
        {
            DashHubContext.Clients.All.ping();
        }

        public void Stop(bool immediate)
        {
            Timer.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }
    }
}
