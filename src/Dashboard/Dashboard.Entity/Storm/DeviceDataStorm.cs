using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Entity.Storm
{
    /// <summary>
    /// Données de capture d'un device provenant de storm
    /// </summary>
    public class DeviceDataStorm
    {
        public string Device { get; set; }
        public string Datas { get; set; }
        public string Timestamp { get; set; }
        public string Unit { get; set; }
        public string DeviceName { get; set; }
    }
}
