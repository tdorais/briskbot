using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace briskbot.access
{
    public class GameAccess : IGameAccess
    {        
        private IApiClient client;

        public GameAccess(IApiClient openClient)
        {
            client = openClient;
        }

        public async Task<GameResult> CreateGame(string teamName)
        {
            string url = "/v1/brisk/game";
            StringContent content = new StringContent($"{{sdf\"join\": true, \"team_name\": \"{teamName}\"}}"); //refactor into serialized object
            
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
            string content = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;

            if(!response.IsSuccessStatusCode)
                throw new Exception($"{(int)response.StatusCode} {response.ReasonPhrase}: {content}");

            T result = JsonConvert.DeserializeObject<T>(content);

            return result;           
        }

        private async Task<T> Post<T>(string url, HttpContent httpContent)
        {
            HttpResponseMessage response = await client.Post(url, httpContent);

            string content = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;

            if(!response.IsSuccessStatusCode)
                throw new Exception($"{(int)response.StatusCode} {response.ReasonPhrase}: {content}");

            T result = JsonConvert.DeserializeObject<T>(content);

            return result;           
        }
    }
}
