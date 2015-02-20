//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client
using System.Collections.Generic;

namespace PowerBI.Api.Client.Entity
{
    /// <summary>
    /// Rows specific to DeviceData table
    /// </summary>
    public sealed class DatasetRows
    {
        public IList<object> Rows { get; set; }
    }
}
