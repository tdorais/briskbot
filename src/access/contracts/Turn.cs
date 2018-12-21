using Newtonsoft.Json;

namespace briskbot.access
{
    [JsonObject]
    public class Turn
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
        public bool? current_turn;
        
        [JsonProperty]
        public bool eliminated;
        
        [JsonProperty]
        public int? winner;
    }
}