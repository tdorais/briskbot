using System;
using System.Net.Http;
using System.Net;
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
            string url = "/v1/brisk/game";
            StringContent content = new StringContent("{\"join\": true, \"team_name\": \"Scaffold Soldiers\"}"); //refactor into serialized object
            
            return await Post<GameResult>(url, content);
        }

        public async Task<PlayerState> GetPlayerState(int gameId, int playerId)
        {
            string url = $"/v1/brisk/game/{gameId}/player/{playerId}";

            return await Get<PlayerState>(url);
        }

        public async Task<Turn> CheckTurn(int gameId, int playerId)
        {
            string url = $"/v1/brisk/game/{gameId}/player/{playerId}?check_turn=true";

            return await Get<Turn>(url);
        }

        public async Task<HttpStatusCode> EndTurn(int gameId, int playerId, string token)
        {
            string url = $"/v1/brisk/game/{gameId}/player/{playerId}";
            StringContent content = new StringContent($"{{\"token\": \"{token}\", \"end_turn\": true}}");

            HttpResponseMessage response = await client.Post(url, content);

            return response.StatusCode;
        }

        private async Task<T> Get<T>(string url)
        {
            HttpResponseMessage response = await client.Get(url);
            string content = await response.Content.ReadAsStringAsync();

            T result = JsonConvert.DeserializeObject<T>(content);

            return result;           
        }

        private async Task<T> Post<T>(string url, HttpContent httpContent)
        {
            HttpResponseMessage response = await client.Post(url, httpContent);
            string content = await response.Content.ReadAsStringAsync();

            T result = JsonConvert.DeserializeObject<T>(content);

            return result;           
        }
    }
}
