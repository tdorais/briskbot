using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace briskbot.access
{
    public class GameAccess
    {        
        private IApiClient client;

        public GameAccess(IApiClient openClient)
        {
            client = openClient;
        }

        public async Task<GameResult> CreateGame()
        {
            HttpResponseMessage response = await client.Post("/v1/brisk/game", new StringContent("{\"join\": true, \"team_name\": \"Scaffold Soldiers\"}"));
            //response.
            string content = await response.Content.ReadAsStringAsync();

            GameResult result = JsonConvert.DeserializeObject<GameResult>(content);
            
            return result;
        }
    }
}
