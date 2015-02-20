//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client
using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
    public sealed class PowerBiConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("Api", IsRequired = true)]
        public Api Api
        {
            get
            {
                return (Api)this["Api"];
            }
        }

        [ConfigurationProperty("OAuth", IsRequired = true)]
        public OAuth OAuth
        {
            get
            {
                return (OAuth)this["OAuth"];
            }
        }
    }
}
