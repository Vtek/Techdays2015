using System;

namespace Dashboard.Entity.Business
{
    /// <summary>
    /// Données provenant d'un device
    /// </summary>
    public class DeviceData
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
        public double Data { get; set; }

        /// <summary>
        /// Date de la capture
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Unité de la données
        /// </summary>
        public string DeviceUnit { get; set; }
    }
}
