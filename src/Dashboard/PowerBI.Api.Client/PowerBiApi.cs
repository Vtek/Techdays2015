//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client
using System;
using System.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PowerBI.Api.Client.Configuration;
using PowerBI.Api.Client.Entity;
using System.Collections.Generic;
using System.Configuration;
using PowerBI.Api.Client.Helper;

namespace PowerBI.Api.Client
{
    /// <summary>
    /// Fluent api design to deal with PowerBI rest service
    /// </summary>
    public sealed class PowerBiApi
    {
        /// <summary>
        /// PowerBI api configuration
        /// </summary>
        public PowerBiConfiguration Configuration { get; private set; }

        /// <summary>
        /// Authentication context
        /// </summary>
        public AuthenticationContext AuthenticationContext { get; private set; }

        /// <summary>
        /// Access token
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Create a new instance of PowerBiApi
        /// </summary>
        public PowerBiApi()
        {
            Configuration = (PowerBiConfiguration)ConfigurationManager.GetSection(typeof(PowerBiConfiguration).Name);
        }

        public PowerBiApi Authenticate()
        {
            if (AuthenticationContext == null)
            {
                var tokenCache = new TokenCache();
                AuthenticationContext = new AuthenticationContext(Configuration.OAuth.Authority, tokenCache);
            }

            var authResult = string.IsNullOrEmpty(AccessToken) 
                ? AuthenticationContext.AcquireToken(Configuration.OAuth.Resource,Configuration.OAuth.Client, new UserCredential(Configuration.OAuth.User, Configuration.OAuth.Password))
                : AuthenticationContext.AcquireTokenSilent(Configuration.OAuth.Resource, Configuration.OAuth.Client);

            AccessToken = authResult.AccessToken;

            return this;
        }

        public List<Dataset> GetAll()
        {
            return new ApiClient(AccessToken).Get<DatasetCollection>(Configuration.Api.Url).Datasets;
        }

        public bool Exist(string datasetName)
        {
            return GetAll().Any(x => x.Name == datasetName);
        }

        public string GetDatasetId(string datasetName)
        {
            return GetAll().First(x => x.Name == datasetName).Id;
        }

        /// <summary>
        /// Create a datasetName which contains specified table types
        /// </summary>
        /// <param name="datasetName"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public bool Create(string datasetName, params Type[] types)
        {
            try
            {
                var dataset = DefinitionHelper.GetDataset(datasetName);

                foreach (var type in types)
                    dataset.Tables.Add(DefinitionHelper.GetTable(type));

                return new ApiClient(AccessToken).Post(Configuration.Api.Url, dataset);
            }
            catch
            {
                return false;
            }
        }

        public bool Send(string datasetId, object obj)
        {
            var datasetRows = GetDatasetRows(obj);
            var url = string.Format("{0}/{1}/tables/{2}/rows", Configuration.Api.Url, datasetId, obj.GetType().Name);
            return new ApiClient(AccessToken).Post(url, datasetRows);
        }

        public bool Delete<T>(string datasetId)
        {
            var url = string.Format("{0}/{1}/tables/{2}/rows", Configuration.Api.Url, datasetId, typeof(T).Name);
            return new ApiClient(AccessToken).Delete(url);
        }

        private DatasetRows GetDatasetRows(object obj)
        {
            return new DatasetRows
            {
                Rows = (obj is IList<object>) ? (IList<object>) obj : new List<object> { obj }
            };
        }

        public void Do(Action<PowerBiApi> action)
        {
            action(this);
        }
    }
}
