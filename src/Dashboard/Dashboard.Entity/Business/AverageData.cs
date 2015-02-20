using System;

namespace Dashboard.Entity.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class AverageData
    {
        /// <summary>
        /// Identifiant du device
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Nom du device
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Donnée contenu dans la capture
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// Date de la capture
        /// </summary>
        public DateTime Date { get; set; }
    }
}
