using Newtonsoft.Json;

namespace briskbot.access
{
    [JsonObject]
    public class PlayerState
    {        
        [JsonProperty]
        public string version;
         
        [JsonProperty]      
        public string service;
        
        [JsonProperty]
        public bool? current_turn;
        
        [JsonProperty]
        public int? winner;
        
        [JsonProperty]
        public bool eliminated;
        
        [JsonProperty]
        public int turns_taken;
        
        [JsonProperty]
        public int num_armies;
        
        [JsonProperty]
        public int num_reserves;
        
        [JsonProperty]
        public Territory[] territories;
    }
}