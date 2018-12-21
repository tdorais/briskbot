using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using briskbot.access;

namespace briskbot
{
    class Program
    {
        static void Main(string[] args)
        {

            Uri endpoint = new Uri("http://www.briskchallenge.com");

            using(var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = endpoint;
                ApiClient client = new ApiClient(httpClient);             
                GameAccess access = new GameAccess(client);       

                GameResult result = access.CreateGame().GetAwaiter().GetResult();

                Console.WriteLine($"Valar Morghulis: {result.game}");
            }
        }
    }
}
