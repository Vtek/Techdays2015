namespace Dashboard.Entity.Storm
{
    /// <summary>
    /// Données de moyenne des captures d'un device provenant de storm
    /// </summary>
    public sealed class AverageDataStorm
    {
        /// <summary>
        /// Device id
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// Moyenne des captures sur les 10 dernières secondes
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// Date de la dernière capture
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Nom du device
        /// </summary>
        public string DeviceName { get; set; }
    }
}
