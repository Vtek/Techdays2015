//This is the alpha version of PowerBI API Client. 
//This version is obsolete, pls use the release one. 
//More informations : https://github.com/Vtek/PowerBI.Api.Client
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;

namespace PowerBI.Api.Client
{
    /// <summary>
    /// Api client
    /// </summary>
    public class ApiClient
    {
        /// <summary>
        /// Access token
        /// </summary>
        private string AccessToken { get; set; }

        /// <summary>
        /// Create a new instance of ApiClient
        /// </summary>
        /// <param name="accessToken">Access token</param>
        public ApiClient(string accessToken)
        {
            AccessToken = accessToken;
        }

        /// <summary>
        /// Get the specify type data from an url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public T Get<T>(string url)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", AccessToken));
            var responseTask = httpClient.GetAsync(url);
            responseTask.Wait();
            var jsonTask = responseTask.Result.Content.ReadAsStringAsync();
            jsonTask.Wait();

            return JsonConvert.DeserializeObject<T>(jsonTask.Result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver()});
        }

        public bool Post(string url, object obj)
        {
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", AccessToken));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseTask = httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            responseTask.Wait();
            return responseTask.Result.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }

        public bool Delete(string url)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", AccessToken));
            var responseTask = httpClient.DeleteAsync(url);
            responseTask.Wait();
            return responseTask.Result.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }
    }
}
