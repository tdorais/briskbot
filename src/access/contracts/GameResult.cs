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

    public class GameState
    {
        
        [JsonProperty]
        public string version;
        
        [JsonProperty]
        public string service;
        
        [JsonProperty]
        public int game;
        
        [JsonProperty]
        public int num_players;
        public int num_turns_taken;
        public int num_armies;
        public Territory[] territories;
    }

    public class Territory
    {
        
        [JsonProperty]
        public int territory;
        
        [JsonProperty]
        public int player;
        
        [JsonProperty]
        public int num_armies;
    }
}