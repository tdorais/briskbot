using Newtonsoft.Json;

namespace briskbot.access
{
    [JsonObject]
    public class GameResult
    {
        [JsonProperty]
        public string version;

        [JsonProperty]
        public string service;
        
        [JsonProperty]
        public int game;
        
        [JsonProperty]
        public int player;
        
        [JsonProperty]
        public string token;
    }
}