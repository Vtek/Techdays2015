//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client

using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
    /// <summary>
    /// OAuth configuration element
    /// </summary>
    public class OAuth : ConfigurationElement
    {
        /// <summary>
        /// OAuth authority
        /// </summary>
        [ConfigurationProperty("Authority", DefaultValue = "https://login.windows.net/common/oauth2/authorize", IsRequired = true)]
        public string Authority
        {
            get { return (string)this["Authority"]; }
        }

        /// <summary>
        /// Resource for the token
        /// </summary>
        [ConfigurationProperty("Resource", DefaultValue = "https://analysis.windows.net/powerbi/api", IsRequired = true)]
        public string Resource
        {
            get { return (string)this["Resource"]; }
        }

        /// <summary>
        /// ClientId for the token
        /// </summary>
        [ConfigurationProperty("Client", DefaultValue = "", IsRequired = true)]
        public string Client
        {
            get { return (string)this["Client"]; }
        }

        /// <summary>
        /// User
        /// </summary>
        [ConfigurationProperty("User", DefaultValue = "", IsRequired = true)]
        public string User
        {
            get { return (string)this["User"]; }
        }

        /// <summary>
        /// Password
        /// </summary>
        [ConfigurationProperty("Password", DefaultValue = "", IsRequired = true)]
        public string Password
        {
            get { return (string)this["Password"]; }
        }
    }
}
