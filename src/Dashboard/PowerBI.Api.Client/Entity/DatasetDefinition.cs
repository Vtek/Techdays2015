//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client
using System.Collections.Generic;

namespace PowerBI.Api.Client.Entity
{
    /// <summary>
    /// Dataset definition
    /// </summary>
    public class DatasetDefinition
    {
        /// <summary>
        /// Dataset name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dataset tables
        /// </summary>
        public List<TableDefinition> Tables { get; set; }
    }
}
