using System.Collections.Generic;

namespace Dashboard.Entity.Rows
{
    /// <summary>
    /// Rows specific to DeviceData table
    /// </summary>
    public sealed class DatasetRows<T> where T : class
    {
        public IList<T> Rows { get; set; }
    }
}
