using System.Collections.Generic;

namespace briskbot.access
{
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