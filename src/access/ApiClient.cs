using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace briskbot.access
{
    public class ApiClient : IApiClient
    {
        private HttpClient client;

        public ApiClient(HttpClient openClient)
        {
            client = openClient;
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> Post(string url, HttpContent content)
        {            
            return await client.PostAsync(url, content);
        }
    }
}