using Newtonsoft.Json;

namespace briskbot.access
{
    [JsonObject]
    public class Territory
    {
        [JsonProperty]
        public int territory;
        
        [JsonProperty]
        public int player;
                
        [JsonProperty]
        public string territory_name;

        [JsonProperty]
        public int num_armies;
    }
}