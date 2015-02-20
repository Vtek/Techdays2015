//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client
namespace PowerBI.Api.Client.Entity
{
    /// <summary>
    /// Column definition
    /// </summary>
    public class ColumnDefinition
    {
        /// <summary>
        /// Column name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Column datatype
        /// </summary>
        public string DataType { get; set; }
    }
}
