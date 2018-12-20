using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace briskbot
{
    class Program
    {
        static void Main(string[] args)
        {

            Uri endpoint = new Uri("http://www.briskchallenge.com");

            try
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = endpoint;

                    Task<int> gameId = CreateGame(client);
                    gameId.Wait();

                    Console.WriteLine($"Valar Morghulis: {gameId.Result}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something terrible happened: {ex.ToString()}");
            }

        }

        static async Task<int> CreateGame(HttpClient client)
        {
            HttpResponseMessage response = await client.PostAsync("/v1/brisk/game", new StringContent("{\"join\": true, \"team_name\": \"Scaffold Soldiers\"}") );
            string content = await response.Content.ReadAsStringAsync();
            //JObject result = JObject.Parse(content);
            GameResult result = JsonConvert.DeserializeObject<GameResult>(content);
            
            return result.game;
        }

        // static async Task<GameState> GetGameState(HttpClient client)
        // {

        // }
    }

    public class GameResult
    {
        public string version {get;set;}
        public string service {get;set;}
        public int game {get;set;}
        public int player {get;set;}
        public string token {get;set;}
    }

    public class GameState
    {
        public string version {get;set;}
        public string service {get;set;}
        public int game {get;set;}
        public int num_players {get;set;}
        public int num_turns_taken {get;set;}
        public int num_armies {get;set;}
        public List<Territory> territories {get;set;}
    }

    public class Territory
    {
        public int territory {get;set;}
        public int player {get;set;}
        public int num_armies {get;set;}
    }
}
