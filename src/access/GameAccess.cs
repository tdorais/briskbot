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

        private GameResult gameInfo;

        public int GameId
        {
            get {
                return gameInfo.game;
            }
        }

        public string Token
        {
            get {
                return gameInfo.token;
            }
        }

        public int CurrentPlayer
        {
            get {
                return gameInfo.player;
            }
        }
        
        public async static Task<IGameAccess> Create(IApiClient openClient, string teamName)
        {
            GameAccess newAccess = new GameAccess(openClient);
            newAccess.gameInfo = await newAccess.CreateGame(teamName);

            return newAccess;
        }

        public GameAccess(IApiClient openClient)
        {
            client = openClient;
            gameInfo = new GameResult() {game = 0, token = "", player = 0};
        }

        public async Task<GameResult> CreateGame(string teamName)
        {
            string url = "/v1/brisk/game";
            StringContent content = new StringContent($"{{\"join\": true, \"team_name\": \"{teamName}\"}}");
            
            return await Post<GameResult>(url, content);
        }

        public async Task<PlayerState> GetPlayerState()
        {
            string url = $"/v1/brisk/game/{GameId}/player/{CurrentPlayer}";

            return await Get<PlayerState>(url);
        }

        public async Task<bool> PlaceArmies(int territory, int num)
        {
            string url = $"/v1/brisk/game/{GameId}/player/{CurrentPlayer}/territory/{territory}";
            StringContent content = new StringContent($"{{\"token\":\"{Token}\", \"num_armies\":{num}}}");

            HttpResponseMessage response = await client.Post(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EndTurn()
        {
            string url = $"/v1/brisk/game/{GameId}/player/{CurrentPlayer}";
            StringContent content = new StringContent($"{{\"token\": \"{Token}\", \"end_turn\": true}}");

            HttpResponseMessage response = await client.Post(url, content);

            return response.IsSuccessStatusCode;
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
