//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client
using System.Collections.Generic;

namespace PowerBI.Api.Client.Entity
{
    /// <summary>
    /// Table definition
    /// </summary>
    public class TableDefinition
    {
        /// <summary>
        /// Table name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tablles columns
        /// </summary>
        public List<ColumnDefinition> Columns { get; set; } 
    }
}
