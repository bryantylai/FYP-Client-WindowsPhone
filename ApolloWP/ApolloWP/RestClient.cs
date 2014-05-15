using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace ApolloWP
{
    public class RestClient
    {
        private HttpClient client;

        /// <summary>
        /// Provides base class for sending HTTP Request and receiving HTTP Response in JSON
        /// </summary>
        public RestClient()
        {
            this.client = new HttpClient();
        }

        /// <summary>
        /// Sending HTTP GET Request
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <param name="url">The url of API to call</param>
        /// <param name="callback">A function to handle the HTTP Response</param>
        public async void Get<T>(string url, Action<T> callback)
        {
            var result = await this.client.GetStringAsync(new Uri(url, UriKind.Absolute));
            callback(JsonConvert.DeserializeObject<T>(result));
        }

        /// <summary>
        /// Sending HTTP GET Request with Basic Authentication
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <param name="url">The url of API to call</param>
        /// <param name="username">The username required for Basic Authentication</param>
        /// <param name="password">The password required for Basic Authentication</param>
        /// <param name="callback">A function to handle the HTTP Response</param>
        public async void Get<T>(string url, string username, string password, Action<T> callback)
        {
            client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Basic", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(username + ":" + password)));
            var result = await this.client.GetStringAsync(new Uri(url, UriKind.Absolute));
            callback(JsonConvert.DeserializeObject<T>(result));
        }

        /// <summary>
        /// Sending HTTP POST Request with Basic Authentication
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <param name="url">The url of API to call</param>
        /// <param name="obj">The object to be serialized into JSON</param>
        /// <param name="callback">A function to handle the HTTP Response</param>
        public async void Post<T>(string url, object obj, Action<T> callback)
        {
            HttpStringContent content = new HttpStringContent(JsonConvert.SerializeObject(obj), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            HttpResponseMessage httpResponseMessage = await client.PostAsync(new Uri(url, UriKind.Absolute), content);
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            callback(JsonConvert.DeserializeObject<T>(result));
        }

        /// <summary>
        /// Sending HTTP POST Request with Basic Authentication
        /// </summary>
        /// <typeparam name="T">The type of element</typeparam>
        /// <param name="url">The url of API to call</param>
        /// <param name="obj">The object to be serialized into JSON</param>
        /// <param name="username">The username required for Basic Authentication</param>
        /// <param name="password">The password required for Basic Authentication</param>
        /// <param name="callback">A function to handle the HTTP Response</param>
        public async void Post<T>(string url, object obj, string username, string password, Action<T> callback)
        {
            HttpStringContent content = new HttpStringContent(JsonConvert.SerializeObject(obj), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Basic", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(username + ":" + password)));
            HttpResponseMessage httpResponseMessage = await client.PostAsync(new Uri(url, UriKind.Absolute), content);
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            callback(JsonConvert.DeserializeObject<T>(result));
        }
    }
}
