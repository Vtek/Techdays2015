//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client

using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
    /// <summary>
    /// Api configuration element
    /// </summary>
    public class Api : ConfigurationElement
    {
        /// <summary>
        /// PowerBI Api url
        /// </summary>
        [ConfigurationProperty("Url", DefaultValue = "https://api.powerbi.com/beta/myorg/datasets", IsRequired = true)]
        public string Url
        {
            get { return (string)this["Url"]; }
        }
    }
}
